﻿/*
Copyright (c) 2016, John Lewin
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies,
either expressed or implied, of the FreeBSD Project.
*/

using MatterHackers.Agg.ImageProcessing;
using MatterHackers.Agg.PlatformAbstract;
using MatterHackers.Agg.UI;
using MatterHackers.Localizations;
using MatterHackers.MatterControl.PrinterControls.PrinterConnections;
using MatterHackers.MatterControl.SlicerConfiguration;
using System;

namespace MatterHackers.MatterControl
{
	public class PrinterSelector : StyledDropDownList
	{
		public event EventHandler AddPrinter;

		public PrinterSelector() : base("Printers".Localize() + "... ")
		{
			UseLeftIcons = true;

			//Add the menu items to the menu itself
			foreach (var printer in ActiveSliceSettings.ProfileData.Profiles)
			{
				this.AddItem(printer.Name, printer.Id.ToString());
			}

			if(ActiveSliceSettings.Instance != null)
			{
				this.SelectedValue = ActiveSliceSettings.Instance.Id();
			}

			this.AddItem(InvertLightness.DoInvertLightness(StaticData.Instance.LoadIcon("icon_circle_plus.png")), "Add New Printer...", "new");

			this.SelectionChanged += (s, e) =>
			{
				int printerID;
				if (int.TryParse(this.SelectedValue, out printerID))
				{
					ActiveSliceSettings.SwitchToProfile(printerID);
				}
				else if(this.SelectedValue == "new")
				{
					if(AddPrinter != null)
					{
						UiThread.RunOnIdle(() => AddPrinter(this, null));
					}
				}
			};
		}
	}
}