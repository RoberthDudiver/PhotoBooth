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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace FluidKit.Controls
{
	public partial class ElementFlow
	{
		#region Mesh Creation

		private GeometryModel3D CreateMeshModel(Visual visual)
		{
			GeometryModel3D model3d = (InternalResources["ElementModel"] as GeometryModel3D).Clone();
			VisualBrush brush;
			if (UseReflection)
			{
				brush = CreateElementReflection(visual);
			}
			else
			{
				brush = new VisualBrush(visual);
			}

		    RenderOptions.SetCachingHint(brush, CachingHint.Cache);
			(model3d.Geometry as MeshGeometry3D).Positions = CreateMeshPositions();
			(model3d.Material as DiffuseMaterial).Brush = brush;

			return model3d;
		}

		private Point3DCollection CreateMeshPositions()
		{
			double aspect = ElementWidth/ElementHeight;
			double reflectionFactor = UseReflection ? 1.0 : 0.5;

			Point3DCollection positions = new Point3DCollection();
			positions.Add(new Point3D(-aspect/2, 1*reflectionFactor, 0));
			positions.Add(new Point3D(aspect/2, 1*reflectionFactor, 0));
			positions.Add(new Point3D(aspect/2, -1*reflectionFactor, 0));
			positions.Add(new Point3D(-aspect/2, -1*reflectionFactor, 0));

			return positions;
		}

		/**
         * Creates the visual for reflection
         */

		private VisualBrush CreateElementReflection(Visual visual)
		{
			Rectangle topRect = new Rectangle();
			topRect.Width = ElementWidth;
			topRect.Height = ElementHeight;
			topRect.Fill = new VisualBrush(visual);

			Rectangle bottomRect = new Rectangle();
			bottomRect.Width = ElementWidth;
			bottomRect.Height = ElementHeight;
			VisualBrush brush = new VisualBrush(visual);
			brush.Transform = new ScaleTransform(1, -1, ElementWidth/2, ElementHeight/2);
			bottomRect.Fill = brush;
			Canvas.SetTop(bottomRect, ElementHeight);

			Rectangle overlayRect = new Rectangle();
			overlayRect.Width = ElementWidth;
			overlayRect.Height = ElementHeight;
			overlayRect.Fill = InternalResources["ReflectionBrush"] as Brush;
			Canvas.SetTop(overlayRect, ElementHeight);

			Canvas canvas = new Canvas();
			canvas.Width = ElementWidth;
			canvas.Height = ElementHeight*2;
			canvas.Children.Add(topRect);
			canvas.Children.Add(bottomRect);
			canvas.Children.Add(overlayRect);

			_elementReflections.Add(canvas);

			return new VisualBrush(canvas);
		}

		#endregion
	}
}