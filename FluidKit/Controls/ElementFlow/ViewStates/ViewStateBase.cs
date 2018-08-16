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

namespace FluidKit.Controls
{
	internal abstract class ViewStateBase
	{
		public void SelectElement(ElementFlow owner, int index)
		{
			Storyboard anim;
			for (int leftItem = 0; leftItem < index; leftItem++)
			{
				anim = PrepareItemAnimation(owner, leftItem, ElementFlow.ElementAnimationType.Left);
				owner.AnimateElement(anim);
			}

			anim = PrepareItemAnimation(owner, index, ElementFlow.ElementAnimationType.Selection);
			owner.AnimateElement(anim);

			for (int rightItem = index + 1; rightItem < owner.VisibleChildrenCount; rightItem++)
			{
				anim = PrepareItemAnimation(owner, rightItem, ElementFlow.ElementAnimationType.Right);
				owner.AnimateElement(anim);
			}
		}

		protected virtual Storyboard PrepareItemAnimation(ElementFlow owner, int index, ElementFlow.ElementAnimationType type)
		{
			return null;
		}
	}
}