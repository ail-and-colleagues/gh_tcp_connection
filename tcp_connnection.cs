using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using System.Net.Sockets;

public class TCPCli
{
    public string stat;
    TcpClient client;
    String server;
    Int32 port, timeout;
    public TCPCli(String server, Int32 port, Int32 timeout)
    {
        this.server = server;
        this.port = port;
        this.timeout = timeout;
        this.client = null;
    }
    public bool Communicate(in byte[] sendData, ref byte[] rcvData)
    {
        try
        {
            this.client = new TcpClient(this.server, this.port);
            NetworkStream ns = this.client.GetStream();
            // send data to the Svr.
            ns.Write(sendData, 0, sendData.Length);
            // receive data from the Svr.
            ns.Read(rcvData, 0, rcvData.Length);
            this.stat = "OK";
            return true;
        }
        catch (Exception ex)
        {
            this.stat = ex.ToString();
            return false;
        }
    }
}

namespace gh_tcp_conn
{
    public class tcp_connnection : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public tcp_connnection()
          : base("tcp_conn", "tcp_conn",
            "communicate other applications via tcp.",
            "tcp_connnection", "base")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("ipAddress", "addr", "ip addtess of the svr", GH_ParamAccess.item, "127.0.0.1");
            pManager.AddIntegerParameter("port", "port", "port of the svr", GH_ParamAccess.item, 3141);
            pManager.AddBooleanParameter("run", "run", "pass True to run", GH_ParamAccess.item, false);
            pManager.AddPointParameter("points", "pts", "points for solve", GH_ParamAccess.list, null);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Connection status", "c_stat", "connection status of the Svr", GH_ParamAccess.item);
            pManager.AddPointParameter("Returned point", "p", "Returned point", GH_ParamAccess.item);
            pManager.AddNumberParameter("Returned score", "s", "Returned score", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, "chk");
            string addr = "";
            if (!DA.GetData(0, ref addr)) return;
            int port = 0;
            if (!DA.GetData(1, ref port)) return;
            bool run = false;
            if (!DA.GetData(2, ref run) || !run) return;

            List<Point3d> points = new List<Point3d>();
            if (!DA.GetDataList(3, points)) return;


            // flatten point list to float[]
            List<float> t = new List<float>();

            foreach (Point3d p in points)
            {
                t.Add((float)p.X);
                t.Add((float)p.Y);
                t.Add((float)p.Z);
            }

            // convert float[] to byte[]
            var byteAry = new byte[sizeof(float) * 3 * points.Count];
            Buffer.BlockCopy(t.ToArray(), 0, byteAry, 0, byteAry.Length);

            // prepate byte[] to receive data from the svr.
            int rcvSize = 3;
            var rcvByteAry = new byte[sizeof(float) * rcvSize];
            var rcvData = new float[rcvSize];

            // communicate via tcp
            TCPCli cli = new TCPCli(addr, port, 1000);
            if (!cli.Communicate(byteAry, ref rcvByteAry))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "connection err: " + cli.stat);
                DA.SetData(0, "connection err: " + cli.stat);
                return;
            }
            DA.SetData(0, String.Format("receive {0} bytes from svr.", rcvByteAry.Length));

            Buffer.BlockCopy(rcvByteAry, 0, rcvData, 0, rcvByteAry.Length);
            var returnedPoint = new Point3d(rcvData[0], rcvData[1], rcvData[2]);

            DA.SetData(1, returnedPoint);
            DA.SetData(2, rcvData[0]);

        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("5BBD612F-99D6-4A14-8830-B85975056F62");
    }
}