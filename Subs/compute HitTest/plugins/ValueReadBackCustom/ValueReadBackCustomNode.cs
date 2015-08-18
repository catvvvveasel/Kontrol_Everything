using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.NonGeneric;

using SlimDX;

using FeralTic.DX11.Resources;
using FeralTic.DX11;
using SlimDX.Direct3D11;
using VVVV.Hosting.IO.Pointers;
using System.Threading.Tasks;

namespace VVVV.DX11.Nodes
{
    [PluginInfo(Name = "ReadBack", Category = "DX11.Buffer", Version = "Vector4", Author = "vux", AutoEvaluate = false)]
    public unsafe class ReadBackBufferVector4Node : IPluginEvaluate, IDX11ResourceDataRetriever
    {
        [Input("Input")]
        protected Pin<DX11Resource<IDX11RWStructureBuffer>> FInput;

        [Input("Enabled", DefaultValue = 1, IsSingle = true)]
        protected ISpread<bool> FInEnabled;

        [Output("Output")]
        protected ValueOutput FOutput;

        [Import()]
        protected IPluginHost FHost;

        private DX11StagingStructuredBuffer staging;

        public DX11RenderContext AssignedContext
        {
            get;
            set;
        }

        public event DX11RenderRequestDelegate RenderRequest;


        #region IPluginEvaluate Members
        public void Evaluate(int SpreadMax)
        {
            if (this.FInput.PluginIO.IsConnected && this.FInEnabled[0])
            {
                if (this.RenderRequest != null) { this.RenderRequest(this, this.FHost); }

                if (this.AssignedContext == null)
                {
                    this.FOutput.Length = 0;
                    return;
                }

                IDX11RWStructureBuffer b = this.FInput[0][this.AssignedContext];
                if (b != null)
                {
                    if (this.staging != null && this.staging.ElementCount != b.ElementCount) { this.staging.Dispose(); this.staging = null; }

                    if (this.staging == null)
                    {
                        staging = new DX11StagingStructuredBuffer(this.AssignedContext.Device, b.ElementCount, 16);
                    }

                    this.AssignedContext.CurrentDeviceContext.CopyResource(b.Buffer, staging.Buffer);

                    this.FOutput.Length = b.ElementCount * 4;
                    DataStream ds = staging.MapForRead(this.AssignedContext.CurrentDeviceContext);

                    double* ptr = this.FOutput.Data;

                    for (int i = 0; i < b.ElementCount; i++)
                    {
                        Vector4 v = ds.Read<Vector4>();
                        ptr[i*4] = v.X;
                        ptr[i * 4 + 1] = v.Y;
                        ptr[i * 4 + 2] = v.Z;
                        ptr[i * 4 + 3] = v.W;
                    }

                    staging.UnMap(this.AssignedContext.CurrentDeviceContext);
                }
                else
                {
                    this.FOutput.Length = 0;
                }
            }
            else
            {
                this.FOutput.Length = 0;
            }
        }
        #endregion

    }
}
