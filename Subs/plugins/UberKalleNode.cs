#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.Graph;
using VVVV.Utils.VColor;
using System.Drawing;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "UberKalle", 
	Category = "VVVV", 
	Help = "Sets Background Kalle of Patches and Subpatches",
	Author = "woei",
	Tags = "",
	AutoEvaluate = true)]
	#endregion PluginInfo
	public class UberKalleNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultColor = new double[] {0.9,0.9,0.9,1.0}, AutoValidate = false)]
		public ISpread<RGBAColor> FInput;
		
		[Input("Include Native Nodes", IsSingle = true, AutoValidate = false)]
		public ISpread<bool> FIncl;
		
		[Input("Update", IsSingle = true, IsBang = true)]
		public IDiffSpread<bool> FUpdate;

		[Import()]
		public IHDEHost FHDE;
		
		[Import()]
		public ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (FUpdate[0])
			{
				FInput.Sync();
				FIncl.Sync();
				FLogger.Log(LogType.Debug, FHDE.RootNode.Name);
				SetKalle(FHDE.RootNode[0], FInput[0].Color, FIncl[0]);
			}
		}
		
		private void SetKalle(INode2 patch, Color color, bool includeNative = false)
		{
			int cInt = color.R | color.G<<8 | color.B<<16;
			string msg = "<PATCH bgcolor=\""+cInt.ToString()+"\" />";
			if (patch.HasPatch)
				if ((!patch.NodeInfo.Filename.Contains(FHDE.ExePath)) ||
					includeNative)
					FHDE.SendXMLSnippet(patch.NodeInfo.Filename, msg, false);

			foreach (var p in patch)
				SetKalle(p, color, includeNative);
		}
	}
}
