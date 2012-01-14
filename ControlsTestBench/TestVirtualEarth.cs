using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AD.Windows.Forms.Controls
{
    public partial class TestVirtualEarth : Form
    {
        VESearchLocation loc1 = new VESearchLocation("Piacenza", 9.698744, 45.052392);
        VESearchLocation loc2 = new VESearchLocation("Parma", 10.332515, 44.804446);
        VESearchLocation loc3 = new VESearchLocation("Reggio Emilia", 10.630785, 44.6981);
        VESearchLocation loc4 = new VESearchLocation("Modena", 10.92522, 44.64708);

        public TestVirtualEarth()
        {
            InitializeComponent();
        }

        private void TestVirtualEarth_Load(object sender, EventArgs e)
        {
            virtualEarth1.StartVirtualEarthMap();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<VESearchLocation> locations = new List<VESearchLocation>();

            locations.Add(loc1);
            locations.Add(loc2);
            locations.Add(loc3);
            locations.Add(loc4);

            RouteDirections rd = virtualEarth1.VE_GetDirections(locations, true, true, VERouteMode.Driving, VEDistanceUnit.Kilometers);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            virtualEarth1.VE_AddPushPin(loc1);
            virtualEarth1.VE_AddPushPin(loc2);
            virtualEarth1.VE_AddPushPin(loc3);
            virtualEarth1.VE_AddPushPin(loc4);

            virtualEarth1.VE_SetCenter(loc3);
        }
    }
}
