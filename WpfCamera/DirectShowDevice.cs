using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

using DirectShowLib;

namespace WpfCamera
{
    public class DirectShowDevice : DependencyObject, ISampleGrabberCB, IDisposable
    {
        #region Delegates

        public delegate void FrameReadyEventHandler(object sender, EventArgs e);

        #endregion

        #region Constants

        private const int PixelSize = 3;

        #endregion

        #region Private fields

        public int FrameTime;

        protected ICaptureGraphBuilder2 CaptureGraphBuilder;
        protected byte[] CapturedFrame;
        protected byte[] Frame;
        protected IGraphBuilder GraphBuilder;
        protected IMediaControl MediaControl;
        protected int PreviewDivider;
        protected byte[] PreviewFrame;
        protected ISampleGrabber SampleGrabber;
        protected Thread UpdateThread;
        protected IBaseFilter VideoInput;
        private readonly List<VideoMode> availableVideoModes;
        private bool frameArrived;
        private int height;
        private DsDevice selectedDevice;
        private VideoMode selectedVideoMode;
        private int width;

        #endregion

        #region Properties

        public static ArrayList AvailableDevices
        {
            get
            {
                var devices = new ArrayList();

                foreach (DsDevice d in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice))
                {
                    if (d.Name.ToUpper().IndexOf("SECONDARY") > -1)
                    {
                    }
                    devices.Add(d);
                }
                return devices;
            }
        }

        public bool CameraAvailable
        {
            get
            {
                if (SampleGrabber != null)
                {
                    return true;
                }
                return false;
            }
        }

        public byte[] CapturedPixelMap
        {
            get { return Frame; }
        }

        public byte[] PreviewPixelMap
        {
            get { return PreviewFrame; }
        }

        public string SelectedDeviceMoniker
        {
            get
            {
                if (selectedDevice != null)
                {
                    return selectedDevice.Name;
                }
                return null;
            }
        }

        public VideoMode SelectedVieoMode
        {
            get { return selectedVideoMode; }
        }

        #endregion

        #region Events

        public event FrameReadyEventHandler FrameReady;

        #endregion

        #region Ctors

        public DirectShowDevice()
        {
            PreviewDivider = 1;
            frameArrived = false;
            availableVideoModes = new List<VideoMode>();
            FrameTime = 200;
        }

        public DirectShowDevice(int width, int height, int previewDivider, int maxFramerate)
        {
            PreviewDivider = previewDivider;
            this.width = width;
            this.height = height;
            frameArrived = false;
            availableVideoModes = new List<VideoMode>();
            FrameTime = (1 / maxFramerate) * 1000;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            if (MediaControl != null)
            {
                MediaControl.StopWhenReady();
                Marshal.ReleaseComObject(MediaControl);
            }

            if (GraphBuilder != null)
            {
                Marshal.ReleaseComObject(GraphBuilder);
            }
            if (CaptureGraphBuilder != null)
            {
                Marshal.ReleaseComObject(CaptureGraphBuilder);
            }

            if (UpdateThread != null)
            {
                UpdateThread.Abort();
            }

            CaptureGraphBuilder = null;
            GraphBuilder = null;
            MediaControl = null;
        }

        #endregion

        #region Event triggers

        private void OnFrameReady(object sender, EventArgs e)
        {
            if (FrameReady != null)
            {
                FrameReady(sender, e);
            }
        }

        #endregion

        #region Public methods

        public IBaseFilter GetVideo()
        {
            IBaseFilter baseDevice;

            var filterGraph = new FilterGraph() as IFilterGraph2;

            filterGraph.AddSourceFilterForMoniker(selectedDevice.Mon, null, selectedDevice.Name, out baseDevice);

            IPin pin = DsFindPin.ByCategory(baseDevice, PinCategory.Capture, 0);
            var streamConfig = pin as IAMStreamConfig;
            AMMediaType media;
            int iC = 0, iS = 0;
            streamConfig.GetNumberOfCapabilities(out iC, out iS);
            IntPtr ptr = Marshal.AllocCoTaskMem(iS);
            for (int i = 0; i < iC; i++)
            {
                streamConfig.GetStreamCaps(i, out media, ptr);
                VideoInfoHeader v;
                v = new VideoInfoHeader();
                Marshal.PtrToStructure(media.formatPtr, v);
            }

            Guid iid = typeof (IBaseFilter).GUID;
            object source;
            selectedDevice.Mon.BindToObject(null, null, ref iid, out source);
            return (IBaseFilter)source;
        }

        public List<VideoMode> SelectVideoInput(string deviceName, int width, int height)
        {
            if (UpdateThread != null)
            {
                UpdateThread.Abort();
            }
            this.width = width;
            this.height = height;
            FindDevice(deviceName);
            VideoInput = GetVideo();
            ApplyVideoInput();

            return availableVideoModes;
        }

        public List<VideoMode> SelectVideoInput(string deviceName)
        {
            FindDevice(deviceName);
            VideoInput = GetVideo();
            ApplyVideoInput();

            return availableVideoModes;
        }

        public List<VideoMode> SelectVideoInput()
        {
            if (AvailableDevices.Count == 0)
            {
                return new List<VideoMode>();
            }
            selectedDevice = (DsDevice)AvailableDevices[0];
            VideoInput = GetVideo();
            ApplyVideoInput();

            return availableVideoModes;
        }

        public void SetResolution(int width, int height)
        {
            this.width = width;
            this.height = height;

            object o;

            CaptureGraphBuilder.FindInterface(PinCategory.Capture, MediaType.Video, VideoInput,
                typeof (IAMStreamConfig).GUID, out o);
            var videoStreamConfig = o as IAMStreamConfig;
            AMMediaType media;
            int iC = 0, iS = 0;
            videoStreamConfig.GetNumberOfCapabilities(out iC, out iS);

            IntPtr ptr = Marshal.AllocCoTaskMem(iS);
            int streamId = 0;
            var videoInfo = new VideoInfoHeader();

            availableVideoModes.Clear();
            for (int i = 0; i < iC; i++)
            {
                videoStreamConfig.GetStreamCaps(i, out media, ptr);
                Marshal.PtrToStructure(media.formatPtr, videoInfo);

                if (videoInfo.BmiHeader.Width != 0 && videoInfo.BmiHeader.Height != 0)
                {
                    availableVideoModes.Add(new VideoMode(videoInfo.BmiHeader.Width, videoInfo.BmiHeader.Height));
                }
                if (videoInfo.BmiHeader.Width != width || videoInfo.BmiHeader.Height != height)
                {
                    continue;
                }
                streamId = i;
                selectedVideoMode = availableVideoModes.Last();
                break;
            }

            videoStreamConfig.GetStreamCaps(streamId, out media, ptr);
            Marshal.PtrToStructure(media.formatPtr, videoInfo);
            int hr = videoStreamConfig.SetFormat(media);
            Marshal.FreeCoTaskMem(ptr);
            media.subType = MediaSubType.RGB24;
            media.formatType = FormatType.VideoInfo;
            SampleGrabber.SetMediaType(media);
            DsUtils.FreeAMMediaType(media);
        }

        #endregion

        #region Protected methods

        protected void UpdateBuffer()
        {
            while (true)
            {
                for (int y = 1; y <= height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int flippedPosition = (((height - y) * width) + (x)) * PixelSize;
                        int position = (((y - 1) * width) + (x)) * PixelSize;

                        Frame[flippedPosition + 0] = CapturedFrame[position + 0];
                        Frame[flippedPosition + 1] = CapturedFrame[position + 1];
                        Frame[flippedPosition + 2] = CapturedFrame[position + 2];

                        if (x % PreviewDivider == 0 && y % PreviewDivider == 0)
                        {
                            int previewPosition = (((height - y) * width / PreviewDivider / PreviewDivider) +
                                (x / PreviewDivider)) * PixelSize;

                            PreviewFrame[previewPosition + 0] = CapturedFrame[position + 0];
                            PreviewFrame[previewPosition + 1] = CapturedFrame[position + 1];
                            PreviewFrame[previewPosition + 2] = CapturedFrame[position + 2];
                        }
                    }
                }

                OnFrameReady(this, EventArgs.Empty);
                frameArrived = false;
                while (!frameArrived)
                {
                    Thread.Sleep(FrameTime);
                }
            }
        }

        #endregion

        #region Private methods

        private void ApplyVideoInput()
        {
            Dispose();
            Frame = new byte[(width * height) * PixelSize];
            CapturedFrame = new byte[(width * height) * PixelSize];
            PreviewFrame = new byte[(width / PreviewDivider * height / PreviewDivider) * PixelSize];
            if (VideoInput == null)
            {
                return;
            }
            GraphBuilder = (IGraphBuilder)new FilterGraph();
            CaptureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            MediaControl = (IMediaControl)GraphBuilder;
            CaptureGraphBuilder.SetFiltergraph(GraphBuilder);
            SampleGrabber = new SampleGrabber() as ISampleGrabber;
            GraphBuilder.AddFilter((IBaseFilter)SampleGrabber, "Render");
            SetResolution(width, height);
            GraphBuilder.AddFilter(VideoInput, "Camera");
            SampleGrabber.SetBufferSamples(false);
            SampleGrabber.SetOneShot(false);
            SampleGrabber.GetConnectedMediaType(new AMMediaType());
            SampleGrabber.SetCallback(this, 1);
            CaptureGraphBuilder.RenderStream(PinCategory.Preview, MediaType.Video, VideoInput, null,
                SampleGrabber as IBaseFilter);

            if (UpdateThread != null)
            {
                UpdateThread.Abort();
            }
            UpdateThread = new Thread(UpdateBuffer);
            UpdateThread.Start();
            MediaControl.Run();
            Marshal.ReleaseComObject(VideoInput);
        }

        private void FindDevice(string deviceName)
        {
            if (AvailableDevices.Count == 0)
            {
                return;
            }

            bool deviceFound = false;
            foreach (DsDevice device in AvailableDevices)
            {
                if (device.Name == deviceName)
                {
                    selectedDevice = device;
                    deviceFound = true;
                }
            }

            if (!deviceFound)
            {
                selectedDevice = (DsDevice)AvailableDevices[0];
            }
        }

        #endregion

        #region ISampleGrabberCB Members

        public int BufferCB(double sampleTime, IntPtr pBuffer, int bufferLen)
        {
            Marshal.Copy(pBuffer, CapturedFrame, 0, bufferLen);
            frameArrived = true;
            return 0;
        }

        public int SampleCB(double sampleTime, IMediaSample pSample)
        {
            return 0;
        }

        #endregion
    }
}