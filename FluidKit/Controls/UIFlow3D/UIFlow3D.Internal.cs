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
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls.UIFlow3D
{
    public partial class UIFlow3D
    {
        private Viewport2DVisual3D CreateMeshModel(Visual visualElement)
        {
            var model = new Viewport2DVisual3D
            {
                Geometry = new MeshGeometry3D
                {
                    TriangleIndices = new Int32Collection(
                        new int[] { 0, 1, 2, 2, 3, 0 }),
                    TextureCoordinates = new PointCollection(
                        new Point[] 
                            { 
                                new Point(0, 1), 
                                new Point(1, 1), 
                                new Point(1, 0), 
                                new Point(0, 0) 
                            }),
                    Positions = CreateMeshPositions(visualElement as FrameworkElement)
                },
                Material = new DiffuseMaterial(),

                // Host the 2D element in the 3D model.
                Visual = visualElement
            };

            model.Transform = (InternalResources["Transfrom3DGroup"] as Transform3DGroup).Clone();

            Viewport2DVisual3D.SetIsVisualHostMaterial(model.Material, true);

            return model;
        }

        private Point3DCollection CreateMeshPositions(FrameworkElement visualElement)
        {
            double aspect;

            if (visualElement == null || double.IsNaN(visualElement.Width) || double.IsNaN(visualElement.Height))
            {
                aspect = ElementWidth / ElementHeight;
            }
            else
                aspect = visualElement.Width / visualElement.Height;

            double factor = 0.5;

            Point3DCollection positions = new Point3DCollection();
            positions.Add(new Point3D(-aspect / 2, -1 * factor, 0));
            positions.Add(new Point3D(aspect / 2, -1 * factor, 0));
            positions.Add(new Point3D(aspect / 2, 1 * factor, 0));
            positions.Add(new Point3D(-aspect / 2, 1 * factor, 0));

            return positions;
        }
    }	
}