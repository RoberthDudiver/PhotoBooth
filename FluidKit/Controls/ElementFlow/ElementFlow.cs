// -------------------------------------------------------------------------------
// 
// This file is part of the FluidKit project: http://www.codeplex.com/fluidkit
// 
// Copyright (c) 2008, Pavan Podila 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this 
// list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice, this 
// list of conditions and the following disclaimer in the documentation and/or 
// other materials provided with the distribution.
// 
// * Neither the name of FluidKit nor the names of its contributors may be used to 
// endorse or promote products derived from this software without specific prior 
// written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR 
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON 
// ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
// -------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls
{
	public partial class ElementFlow : Panel
	{
		// Fields

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

		public ElementFlow()
		{
			CreateContainerVisuals();
			SetupEventHandlers();

			_currentViewState = CoverFlowState;
		}

		private void SetupEventHandlers()
		{
			Loaded += ElementFlow_Loaded;
            DataContextChanged += new DependencyPropertyChangedEventHandler(ElementFlow_DataContextChanged);
            
		}

        private void ElementFlow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //ReflowItems();
        }

		private void CreateContainerVisuals()
		{
			_elementReflections = new List<Canvas>();
			PrepareViewport();
			InternalResources = _viewport.Resources;
		}

		#endregion

		#region Event Handlers

		private void ElementFlow_Loaded(object sender, RoutedEventArgs e)
		{
			// Do Post load initializations
			DoPostLoadInit();
			SelectItemCore(SelectedIndex);
		}

		private void DoPostLoadInit()
		{
			// Viewport initialization - Adjust the camera
			PerspectiveCamera camera = _viewport.Camera as PerspectiveCamera;
			if (UseReflection)
			{
				camera.Position = new Point3D(0, 0.5, 4);
				camera.LookDirection = new Vector3D(0, 0, -4);
			}
			else
			{
				camera.Position = new Point3D(0, 0, 4);
				camera.LookDirection = new Vector3D(0, 0, -4);
			}
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
				int modelIndex = _modelGroup.Children.IndexOf(_hitModel);
				_hitModel = null;
				return modelIndex;
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
				Application.LoadComponent(new Uri("/FluidKit;component/Controls/ElementFlow/Viewport.xaml", UriKind.Relative)) as
				Viewport3D;

			// ModelGroup for containing the mesh models of elements
			_modelGroup = _viewport.FindName("ModelGroup") as Model3DGroup;
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

		internal Storyboard PrepareTemplateStoryboard(int index)
		{
			// Initialize storyboard
			Storyboard sb = (InternalResources["ItemAnimator"] as Storyboard).Clone();
			Rotation3DAnimation rotAnim = sb.Children[0] as Rotation3DAnimation;
			Storyboard.SetTargetName(rotAnim, ElementIdentifier + index);

			DoubleAnimation xAnim = sb.Children[1] as DoubleAnimation;
			Storyboard.SetTargetName(xAnim, ElementIdentifier + index);

			DoubleAnimation yAnim = sb.Children[2] as DoubleAnimation;
			Storyboard.SetTargetName(yAnim, ElementIdentifier + index);

			DoubleAnimation zAnim = sb.Children[3] as DoubleAnimation;
			Storyboard.SetTargetName(zAnim, ElementIdentifier + index);

			return sb;
		}

	public void AnimateElement(Storyboard sb)
		{
			sb.Begin(_viewport, HandoffBehavior.SnapshotAndReplace);
          
		}



      private static void OnAnimationCompleted(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {

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
			Size eltSize = new Size(ElementWidth, ElementHeight);
			// Arrange children so that their visualbrush has some width/height
			foreach (UIElement child in Children)
			{
				child.Arrange(new Rect(new Point(), eltSize));
			}

			_viewport.Arrange(new Rect(new Point(), finalSize));

			return finalSize;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			Size eltSize = new Size(ElementWidth, ElementHeight);
			foreach (UIElement child in Children)
			{
				child.Measure(eltSize);
			}

			_viewport.Measure(availableSize);

            if (double.IsPositiveInfinity(availableSize.Width))
                availableSize.Width = 0;


            if (double.IsPositiveInfinity(availableSize.Height))
                availableSize.Height = 0;

			return availableSize;
		}

		protected override Visual GetVisualChild(int index)
		{
			if (index == 0)
			{
				return _viewport;
			}
			else
			{
				throw new Exception("Bad index");
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
		private List<Canvas> _elementReflections;
		private GeometryModel3D _hitModel;
		private ResourceDictionary _internalResources;
		private Model3DGroup _modelGroup;
		private Viewport3D _viewport;

		#region Dependency Properties

		public static readonly DependencyProperty ElementHeightProperty =
			DependencyProperty.Register("ElementHeight", typeof (double), typeof (ElementFlow),
			                            new FrameworkPropertyMetadata(300.0));

		public static readonly DependencyProperty ElementWidthProperty =
			DependencyProperty.Register("ElementWidth", typeof (double), typeof (ElementFlow),
			                            new FrameworkPropertyMetadata(400.0));

		public static readonly DependencyProperty FrontItemGapProperty =
			DependencyProperty.Register("FrontItemGap", typeof (double), typeof (ElementFlow),
			                            new PropertyMetadata(0.65, OnFrontItemGapChanged));

		public static readonly DependencyProperty ItemGapProperty =
			DependencyProperty.Register("ItemGap", typeof (double), typeof (ElementFlow),
			                            new PropertyMetadata(0.25, OnItemGapChanged));

		private static readonly DependencyProperty ItemIndexProperty =
			DependencyProperty.Register("ItemIndex", typeof (int), typeof (ElementFlow));

		public static readonly DependencyProperty PopoutDistanceProperty =
			DependencyProperty.Register("PopoutDistance", typeof (double), typeof (ElementFlow),
			                            new PropertyMetadata(1.0, OnPopoutDistanceChanged));

		public static readonly DependencyProperty SelectedIndexProperty =
			DependencyProperty.Register("SelectedIndex", typeof (int), typeof (ElementFlow),
			                            new PropertyMetadata(-1, OnSelectedIndexChanged));

		public static readonly DependencyProperty TiltAngleProperty =
			DependencyProperty.Register("TiltAngle", typeof (double), typeof (ElementFlow),
			                            new PropertyMetadata(45.0, OnTiltAngleChanged));

		public static readonly DependencyProperty UseReflectionProperty =
			DependencyProperty.Register("UseReflection", typeof (bool), typeof (ElementFlow),
			                            new FrameworkPropertyMetadata(true));

		public static readonly DependencyProperty ViewModeProperty =
			DependencyProperty.Register("ViewMode", typeof (ViewModeType), typeof (ElementFlow),
			                            new FrameworkPropertyMetadata(ViewModeType.Coverflow, OnViewModeChanged));




		#endregion

		#region DependencyProperty PropertyChange Callbacks

		private static void OnTiltAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ElementFlow cf = d as ElementFlow;
			cf.ReflowItems();
		}

		private static void OnItemGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ElementFlow ef = d as ElementFlow;
			ef.ReflowItems();
		}

		private static void OnFrontItemGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ElementFlow ef = d as ElementFlow;
			ef.ReflowItems();
		}

		private static void OnPopoutDistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ElementFlow ef = d as ElementFlow;
			ef.ReflowItems();
		}

		private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ElementFlow ef = d as ElementFlow;
			if (ef.IsLoaded == false)
			{
				return;
			}

			ef.SelectItemCore((int) e.NewValue);
		}

		private static void OnViewModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ElementFlow ef = d as ElementFlow;
			ef.ChangeViewState((ViewModeType) e.NewValue);
			ef.ReflowItems();
		}

		#endregion

		#region Properties

		public int SelectedIndex
		{
			get { return (int) GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public double TiltAngle
		{
			get { return (double) GetValue(TiltAngleProperty); }
			set { SetValue(TiltAngleProperty, value); }
		}

		public double ItemGap
		{
			get { return (double) GetValue(ItemGapProperty); }
			set { SetValue(ItemGapProperty, value); }
		}

		public double FrontItemGap
		{
			get { return (double) GetValue(FrontItemGapProperty); }
			set { SetValue(FrontItemGapProperty, value); }
		}

		public double PopoutDistance
		{
			get { return (double) GetValue(PopoutDistanceProperty); }
			set { SetValue(PopoutDistanceProperty, value); }
		}

		public ViewModeType ViewMode
		{
			get { return (ViewModeType) GetValue(ViewModeProperty); }
			set { SetValue(ViewModeProperty, value); }
		}

		public double ElementWidth
		{
			get { return (double) GetValue(ElementWidthProperty); }
			set { SetValue(ElementWidthProperty, value); }
		}

		public double ElementHeight
		{
			get { return (double) GetValue(ElementHeightProperty); }
			set { SetValue(ElementHeightProperty, value); }
		}

		public bool UseReflection
		{
			get { return (bool) GetValue(UseReflectionProperty); }
			set { SetValue(UseReflectionProperty, value); }
		}

		private ResourceDictionary InternalResources
		{
			get { return _internalResources; }
			set { _internalResources = value; }
		}

		/* This gives an accurate count of the number of visible children. Panel.Children is not
         * always accurate one and is generally off-by-one.
         */

		internal int VisibleChildrenCount
		{
			get { return _modelGroup.Children.Count; }
		}

		#endregion

       

		protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            
            
			if (visualAdded != null)
			{
				UIElement elt = visualAdded as UIElement;
				GeometryModel3D model = CreateMeshModel(elt);
				_modelGroup.Children.Add(model);
				UpdateElementIndexes(elt, true);
				if (IsLoaded)
				{
					ReflowItems();
				}
			}

			if (visualRemoved != null)
			{
				UIElement elt = visualRemoved as UIElement;
				int index = (int) elt.GetValue(ItemIndexProperty);

                //if (_modelGroup.Children.Count - 1 <= index)
                //{
                    GeometryModel3D model = _modelGroup.Children[index] as GeometryModel3D;

                    UpdateElementIndexes(elt, false);

                    _modelGroup.Children.Remove(model);
                //}
                //else
                //    return;
				


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
            
           NameScope scope = ((NameScope)NameScope.GetNameScope(_viewport));
            
		    
			if (attach)
			{
				// Register name 
				int childIndex = VisibleChildrenCount - 1;
				GeometryModel3D model = _modelGroup.Children[childIndex] as GeometryModel3D;
				scope.RegisterName(ElementIdentifier + childIndex, model);
				affectedElement.SetValue(ItemIndexProperty, childIndex);
			}
			else // Element was detached
			{
				int index = (int) affectedElement.GetValue(ItemIndexProperty);
			    
				// Unregister the previous set of names
				for (int i = index; i < VisibleChildrenCount; i++)
				{
					scope.UnregisterName(ElementIdentifier + i);
				}

				// Re-register names and indexes
				int newIndex = index;
				for (int i = index + 1; i < VisibleChildrenCount; i++, newIndex++)
				{
                    UIElement elt = Children[i];
                    elt.SetValue(ItemIndexProperty, newIndex);
                    scope.RegisterName(ElementIdentifier + newIndex, _modelGroup.Children[i]);
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