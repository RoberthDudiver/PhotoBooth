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
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace FluidKit.Controls.UIFlow3D
{
	internal class TimeMachineViewState : ViewStateBase
	{
        protected override Storyboard PrepareItemAnimation(UIFlow3D owner, int index, UIFlow3D.ElementAnimationType type)
		{
			// Initialize storyboard
            Storyboard sb = (owner.InternalResources["ItemAnimator"] as Storyboard).Clone();
            owner.PrepareTemplateStoryboard(sb, index);

			// Child animations
			Rotation3DAnimation rotAnim = sb.Children[0] as Rotation3DAnimation;
			DoubleAnimation xAnim = sb.Children[1] as DoubleAnimation;
			DoubleAnimation yAnim = sb.Children[2] as DoubleAnimation;
			DoubleAnimation zAnim = sb.Children[3] as DoubleAnimation;

			switch (type)
			{
                case UIFlow3D.ElementAnimationType.Left:
					xAnim.To = -1*owner.ItemGap*(owner.SelectedIndex - index);
					zAnim.To = owner.ItemGap*(owner.SelectedIndex - index);
					break;
                case UIFlow3D.ElementAnimationType.Selection:
					(rotAnim.To as AxisAngleRotation3D).Angle = -1*45;
					(rotAnim.To as AxisAngleRotation3D).Axis = new Vector3D(0, 0, 1);
					xAnim.To = 2*owner.ItemGap;
					break;
                case UIFlow3D.ElementAnimationType.Right:
					xAnim.To = owner.ItemGap*(index - owner.SelectedIndex);
					zAnim.To = -1*owner.ItemGap*(index - owner.SelectedIndex);
					break;
			}

			return sb;
		}
	}
}