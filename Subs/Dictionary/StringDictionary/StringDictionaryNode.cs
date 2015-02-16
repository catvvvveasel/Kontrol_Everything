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
	[PluginInfo(Name = "Dictionary", Category = "String", Help = "stores spreads of strings assigned to an ID", Tags = "String Buffer ID S+H Queue")]
	#endregion PluginInfo
	public class StringDictionaryNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("SetID")]
		ISpread<string> FSetID;

		[Input ("InputString")]
		ISpread<ISpread<string>> FInputString;
		
		[Input ("GetID")]
		ISpread<string> FGetID;
				
		[Input("ClearBuffer", IsBang = true, IsSingle = true)]
		ISpread<bool> FClearBuffer;
		
		[Input ("Default")]
		ISpread<ISpread<string>> FDefaultString;
		
		[Output ("OutputValue")]
		ISpread<ISpread<string>> FOutputString;
		
		[Output ("Key Count")]
		ISpread<double> FOutputKeys;
		
		[Import()]
		ILogger FLogger;
		
		Dictionary<string, ISpread<string>> d = new Dictionary <string, ISpread<string>>();
		#endregion fields & pins
		public void MkDict()	
		{
			int count;				
			count = Math.Min(FInputString.SliceCount, FSetID.SliceCount);
			
			if (count!= 0)
			{
				for (int i=0; i<FInputString.SliceCount; i++)
				{
				if(d.ContainsKey(FSetID[i]))
					d.Remove(FSetID[i]);				
					
					d.Add(FSetID[i], FInputString[i].Clone());
				}
			}
		}
		public void Evaluate(int SpreadMax)
		{
			if (FClearBuffer[0])
				d.Clear();
				MkDict();
			
			FOutputString.SliceCount=FGetID.SliceCount;
						
			FOutputKeys[0] = d.Count; 
			
			for (int i=0; i<FGetID.SliceCount; i++)
			{
				if (d.ContainsKey(FGetID[i]))
				FOutputString[i] = d[FGetID[i]];
			else
				FOutputString[i] = FDefaultString[i];
			}		
		}
	}
}