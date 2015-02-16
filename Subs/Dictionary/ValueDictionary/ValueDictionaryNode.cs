#region usings
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;


using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "Dictionary", Category = "Value", Help = "Srores Spreads of Values asssiged to an ID", Tags = "Buffer Value Spread ID S+H Queue")]
	#endregion PluginInfo
	public class ValueDictionaryNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("SetID")]
		ISpread<string> FSetID;

		[Input ("InputValue")]
		ISpread<ISpread<double>> FInputValue;
		
		[Input ("GetID")]
		ISpread<string> FGetID;
				
		[Input("ClearBuffer", IsBang = true, IsSingle = true)]
		ISpread<bool> FClearBuffer;
		
		[Input ("Default")]
		ISpread<ISpread<double>> FDefault;
		
		[Output ("OutputValue")]
		ISpread<ISpread<double>> FOutputValue;
		
		[Output ("KeyCount")]
		ISpread<double> FOutputKeyCount;

		
		[Import()]
		ILogger FLogger;
		
		Dictionary<string, ISpread<double>> d = new Dictionary <string, ISpread<double>>();
		#endregion fields & pins
		public void MkDict()	
		{
			int count;				
			count = Math.Min(FInputValue.SliceCount, FSetID.SliceCount);
			
			if (count!= 0)
			{
				for (int i=0; i<FInputValue.SliceCount; i++)
				{
				if(d.ContainsKey(FSetID[i]))
					d.Remove(FSetID[i]);				
					
					d.Add(FSetID[i], FInputValue[i].Clone());
				}
			}
		}
		public void Evaluate(int SpreadMax)
		{
			if (FClearBuffer[0])
				d.Clear();

				MkDict();
			
			FOutputValue.SliceCount=FGetID.SliceCount;
						
			FOutputKeyCount[0] = d.Count; 
			
			
			for (int i=0; i<FGetID.SliceCount; i++)
			{
				if (d.ContainsKey(FGetID[i]))
				FOutputValue[i] = d[FGetID[i]];
			else
				FOutputValue[i] = FDefault[i];
			}		
		}
	}
}