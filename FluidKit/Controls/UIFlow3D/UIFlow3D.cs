using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls.UIFlow3D
{
    public partial class UIFlow3D : LogicalPanel
    {
        // Fields
        private bool _hasAddedViewport = false;

        #region ViewModeType enum

        public enum ViewModeType
        {
            Coverflow,
            TimeMachine,
            RollerCoaster,
            Rolodex
        }

        #endregion

        #region Initialization

        public UIFlow3D()
        {
            CreateContainerVisuals();
            SetupEventHandlers();
            _currentViewState = CoverFlowState;
        }

        private void SetupEventHandlers()
        {
            Loaded += UIFlow3D_Loaded;
        }

        private void CreateContainerVisuals()
        {
            PrepareViewport();
            InternalResources = _viewport.Resources;
        }

        #endregion

        #region Event Handlers

        private void UIFlow3D_Loaded(object sender, RoutedEventArgs e)
        {
            // Do Post load initializations
            SelectItemCore(SelectedIndex);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (this.IsFocused == false)
            {
                this.Focus();
            }

            if (e.Delta < 0)
            {
                SelectAdjacentItem(false);
            }
            else if (e.Delta > 0)
            {
                SelectAdjacentItem(true);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                SelectAdjacentItem(false);
            }
            else if (e.Key == Key.Right)
            {
                SelectAdjacentItem(true);
            }
            else if (e.Key == Key.F12)
            {
                ToggleViewMode();
            }
        }

        private void ToggleViewMode()
        {
            if (ViewMode == ViewModeType.Coverflow)
            {
                ViewMode = ViewModeType.TimeMachine;
            }
            else if (ViewMode == ViewModeType.TimeMachine)
            {
                ViewMode = ViewModeType.RollerCoaster;
            }
            else if (ViewMode == ViewModeType.RollerCoaster)
            {
                ViewMode = ViewModeType.Rolodex;
            }
            else if (ViewMode == ViewModeType.Rolodex)
            {
                ViewMode = ViewModeType.Coverflow;
            }

            ChangeViewState(ViewMode);

            ReflowItems();
        }

        private void ChangeViewState(ViewModeType mode)
        {
            switch (mode)
            {
                case ViewModeType.Coverflow:
                    _currentViewState = CoverFlowState;
                    break;
                case ViewModeType.TimeMachine:
                    _currentViewState = TimeMachineState;
                    break;
                case ViewModeType.RollerCoaster:
                    _currentViewState = RollerCoasterState;
                    break;
                case ViewModeType.Rolodex:
                    _currentViewState = RolodexState;
                    break;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            // Set the focus on the control
            if (IsFocused == false)
            {
                Focus();
            }
            /* */
            Point position = e.GetPosition(this);
            int selection = TryViewportHitTest(position);
            if (selection != -1)
            {
                SelectedIndex = selection;
            }
        }

        private int TryViewportHitTest(Point position)
        {
            VisualTreeHelper.HitTest(_viewport, null, Viewport_HitTestResult,
                                     new PointHitTestParameters(position));
            if (_hitModel != null)
            {
                if(_geometryToVisual.ContainsKey(_hitModel.Geometry))
                {
                    int modelIndex = _modelGroup.IndexOf(_geometryToVisual[_hitModel.Geometry]);
                    _hitModel = null;
                    return modelIndex;
                }
            }
            return -1;
        }

        private HitTestResultBehavior Viewport_HitTestResult(HitTestResult result)
        {
            RayMeshGeometry3DHitTestResult rayHTResult = result as RayMeshGeometry3DHitTestResult;
            if (rayHTResult != null)
            {
               _hitModel = rayHTResult.ModelHit as GeometryModel3D;
                return HitTestResultBehavior.Stop;
            }

            return HitTestResultBehavior.Continue;
        }

        #endregion

        #region Viewport configuration

        private void PrepareViewport()
        {
            _viewport =
                Application.LoadComponent(new Uri("/FluidKit;component/Controls/UIFlow3D/Viewport.xaml", UriKind.Relative)) as
                Viewport3D;
        }

        #endregion

        #region Item Selection

        private void SelectItemCore(int index)
        {
            if (index >= 0 && index < VisibleChildrenCount)
            {
                _currentViewState.SelectElement(this, index);
            }
        }

        internal Storyboard PrepareTemplateStoryboard(Storyboard sb, int index)
        {
            // Initialize storyboard
            foreach (Timeline timeLine in sb.Children)
            {
                Storyboard.SetTargetName(timeLine, ElementIdentifier + index);
            }
            
            return sb;
        }

        internal void AnimateElement(Storyboard sb)
        {
            sb.Begin(_viewport, HandoffBehavior.SnapshotAndReplace, true);
        }

        #endregion

        #region Layout overrides

        protected override int VisualChildrenCount
        {
            get
            {
                int count = base.VisualChildrenCount;
                count = (count == 0) ? 0 : 1;
                return count;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _viewport.Arrange(new Rect(new Point(), finalSize));

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!_hasAddedViewport)
            {
                _hasAddedViewport = true;
                AddVisualChild(_viewport);
            }
           
            _viewport.Measure(availableSize);

            return _viewport.DesiredSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0)
            {
                return _viewport;
            }
            else
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            Brush background = this.Background;
            if (background != null)
            {
                dc.DrawRectangle(background, null, new Rect(RenderSize));
            }
        }

        #endregion

        #region Utility functions

        private void ReflowItems()
        {
            SelectItemCore(SelectedIndex);
        }

        private void SelectAdjacentItem(bool isToRight)
        {
            int index = -1;
            if (isToRight == false) // Select previous
            {
                index = Math.Max(-1, SelectedIndex - 1);
            }
            else // Select next
            {
                index = Math.Min(VisibleChildrenCount - 1, SelectedIndex + 1);
            }

            if (index != -1)
            {
                SelectedIndex = index;
            }
        }

        #endregion

        private static readonly ViewStateBase CoverFlowState = new CoverFlowViewState();
        private static readonly string ElementIdentifier = "Element_";
        private static readonly ViewStateBase RollerCoasterState = new RollerCoasterViewState();
        private static readonly ViewStateBase RolodexState = new RolodexViewState();
        private static readonly ViewStateBase TimeMachineState = new TimeMachineViewState();
        private ViewStateBase _currentViewState;
        private GeometryModel3D _hitModel;
        private ResourceDictionary _internalResources;
        private List<Viewport2DVisual3D> _modelGroup = new List<Viewport2DVisual3D>();

        private Dictionary<Geometry3D, Viewport2DVisual3D> _geometryToVisual =
            new Dictionary<Geometry3D, Viewport2DVisual3D>();

        private Viewport3D _viewport;

        #region Dependency Properties

        public static readonly DependencyProperty ElementHeightProperty =
            DependencyProperty.Register("ElementHeight", typeof(double), typeof(UIFlow3D),
                                        new FrameworkPropertyMetadata(300.0));

        public static readonly DependencyProperty ElementWidthProperty =
            DependencyProperty.Register("ElementWidth", typeof(double), typeof(UIFlow3D),
                                        new FrameworkPropertyMetadata(400.0));

        public static readonly DependencyProperty FrontItemGapProperty =
            DependencyProperty.Register("FrontItemGap", typeof(double), typeof(UIFlow3D),
                                        new PropertyMetadata(0.65, OnFrontItemGapChanged));

        public static readonly DependencyProperty ItemGapProperty =
            DependencyProperty.Register("ItemGap", typeof(double), typeof(UIFlow3D),
                                        new PropertyMetadata(0.25, OnItemGapChanged));

        private static readonly DependencyProperty ItemIndexProperty =
            DependencyProperty.Register("ItemIndex", typeof(int), typeof(UIFlow3D));

        public static readonly DependencyProperty PopoutDistanceProperty =
            DependencyProperty.Register("PopoutDistance", typeof(double), typeof(UIFlow3D),
                                        new PropertyMetadata(1.0, OnPopoutDistanceChanged));

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(UIFlow3D),
                                        new PropertyMetadata(-1, OnSelectedIndexChanged));

        public static readonly DependencyProperty TiltAngleProperty =
            DependencyProperty.Register("TiltAngle", typeof(double), typeof(UIFlow3D),
                                        new PropertyMetadata(45.0, OnTiltAngleChanged));

        public static readonly DependencyProperty ViewModeProperty =
            DependencyProperty.Register("ViewMode", typeof(ViewModeType), typeof(UIFlow3D),
                                        new FrameworkPropertyMetadata(ViewModeType.Coverflow, OnViewModeChanged));

        public static readonly DependencyProperty CameraProperty =
            DependencyProperty.Register("Camera", typeof(PerspectiveCamera), typeof(UIFlow3D), new UIPropertyMetadata(null, OnCameraChanged));

        public static readonly DependencyProperty LightProperty =
            DependencyProperty.Register("Light", typeof(Light), typeof(UIFlow3D), new UIPropertyMetadata(null, OnLightChanged));

        #endregion

        #region DependencyProperty PropertyChange Callbacks
        private static void OnLightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Light light = e.NewValue as Light;

            if (light == null)
                return;

            UIFlow3D flow = d as UIFlow3D;

            if (flow == null)
                return;
            
            Model3DGroup lightContainer = flow._viewport.FindName("LightContainer") as Model3DGroup;

            lightContainer.Children.Clear();
            lightContainer.Children.Add(light);
        }

        private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PerspectiveCamera camera = e.NewValue as PerspectiveCamera;
            if (camera == null)
                return;

            UIFlow3D flow = d as UIFlow3D;

            if (flow == null)
                return;

            flow._viewport.Camera = camera;
        }

        private static void OnTiltAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIFlow3D cf = d as UIFlow3D;
            cf.ReflowItems();
        }

        private static void OnItemGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIFlow3D ef = d as UIFlow3D;
            ef.ReflowItems();
        }

        private static void OnFrontItemGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIFlow3D ef = d as UIFlow3D;
            ef.ReflowItems();
        }

        private static void OnPopoutDistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIFlow3D ef = d as UIFlow3D;
            ef.ReflowItems();
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIFlow3D ef = d as UIFlow3D;
            if (ef.IsLoaded == false)
            {
                return;
            }

            ef.SelectItemCore((int)e.NewValue);
        }

        private static void OnViewModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIFlow3D ef = d as UIFlow3D;
            ef.ChangeViewState((ViewModeType)e.NewValue);
            ef.ReflowItems();
        }

        #endregion

        #region Properties
        public Light Light
        {
            get { return (Light)GetValue(LightProperty); }
            set { SetValue(LightProperty, value); }
        }

        public PerspectiveCamera Camera
        {
            get { return (PerspectiveCamera)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public double TiltAngle
        {
            get { return (double)GetValue(TiltAngleProperty); }
            set { SetValue(TiltAngleProperty, value); }
        }

        public double ItemGap
        {
            get { return (double)GetValue(ItemGapProperty); }
            set { SetValue(ItemGapProperty, value); }
        }

        public double FrontItemGap
        {
            get { return (double)GetValue(FrontItemGapProperty); }
            set { SetValue(FrontItemGapProperty, value); }
        }

        public double PopoutDistance
        {
            get { return (double)GetValue(PopoutDistanceProperty); }
            set { SetValue(PopoutDistanceProperty, value); }
        }

        public ViewModeType ViewMode
        {
            get { return (ViewModeType)GetValue(ViewModeProperty); }
            set { SetValue(ViewModeProperty, value); }
        }

        public double ElementWidth
        {
            get { return (double)GetValue(ElementWidthProperty); }
            set { SetValue(ElementWidthProperty, value); }
        }

        public double ElementHeight
        {
            get { return (double)GetValue(ElementHeightProperty); }
            set { SetValue(ElementHeightProperty, value); }
        }

        internal ResourceDictionary InternalResources
        {
            get { return _internalResources; }
            set { _internalResources = value; }
        }

        /* This gives an accurate count of the number of visible children. Panel.Children is not
         * always accurate one and is generally off-by-one.
         */

        internal int VisibleChildrenCount
        {
            get { return _modelGroup.Count; }
        }

        #endregion

        protected override void OnLogicalChildrenChanged(UIElement visualAdded, UIElement visualRemoved)
        {
            if (visualAdded != null)
            {
                UIElement elt = visualAdded;
                Viewport2DVisual3D model = CreateMeshModel(elt);
                _modelGroup.Add(model);
                _viewport.Children.Add(model);
                _geometryToVisual.Add(model.Geometry, model);

                UpdateElementIndexes(elt, true);
                if (IsLoaded)
                {
                    ReflowItems();
                }
            }

            if (visualRemoved != null)
            {
                UIElement elt = visualRemoved;
                int index = (int)elt.GetValue(ItemIndexProperty);

                Viewport2DVisual3D model = _modelGroup[index];

                UpdateElementIndexes(elt, false);

                _modelGroup.Remove(model);
                _viewport.Children.Remove(model);
                _geometryToVisual.Remove(model.Geometry);

                // Update SelectedIndex if needed
                if (SelectedIndex >= 0 && SelectedIndex < VisibleChildrenCount)
                {
                    ReflowItems();
                }
                else
                {
                    SelectedIndex = Math.Max(0, VisibleChildrenCount - 1);
                }
            }
        }

        /**
         * This updates the target-names used for animations and also updates the 
         * ItemIndex dependency property
         */

        private void UpdateElementIndexes(UIElement affectedElement, bool attach)
        {
            NameScope scope = (NameScope)NameScope.GetNameScope(_viewport);

            if (attach)
            {
                // Register name 
                int childIndex = VisibleChildrenCount - 1;
                Viewport2DVisual3D model = _modelGroup[childIndex];
                scope.RegisterName(ElementIdentifier + childIndex, model);
                affectedElement.SetValue(ItemIndexProperty, childIndex);
            }
            else // Element was detached
            {
                int index = (int)affectedElement.GetValue(ItemIndexProperty);

                // Unregister the previous set of names
                for (int i = index; i < VisibleChildrenCount; i++)
                {
                    scope.UnregisterName(ElementIdentifier + i);
                }

                // Re-register names and indexes
                int newIndex = index;
                for (int i = index + 1; i < VisibleChildrenCount; i++, newIndex++)
                {
                    UIElement elt = _modelGroup[i].Visual as UIElement;
                    elt.SetValue(ItemIndexProperty, newIndex);
                    scope.RegisterName(ElementIdentifier + newIndex, _modelGroup[i]);
                }
            }
        }

        #region Nested type: ElementAnimationType

        internal enum ElementAnimationType
        {
            Left,
            Right,
            Selection,
        }

        #endregion
    }
}
