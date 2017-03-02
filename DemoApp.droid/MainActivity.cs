using System;
using Android.App;
using Android.Widget;
using Android.OS;
//Nuevo
using System.Threading.Tasks;
using DemoApp.portable;
using System.Linq;

namespace DemoApp.droid
{
    [Activity(Label = "DemoXamarinAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText txtID;
        ListView listPersonas;
        Button btnBuscar, btnEliminar, btnRegistrar;

        Services controller = new Services();
        Persona persona = new Persona();

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // "Seteamos" nuestra vista Main.axml
            SetContentView(Resource.Layout.Main);

            //Tomamos el id de nuestros layout y hacemos un casting con nuestros objetos respectivamente

            txtID = (EditText)FindViewById(Resource.Id.txtID);
            listPersonas = (ListView)FindViewById(Resource.Id.listPersonas);
            btnRegistrar = (Button)FindViewById(Resource.Id.btnRegistrar);
            btnBuscar = (Button)FindViewById(Resource.Id.btnBuscar);
            btnEliminar = (Button)FindViewById(Resource.Id.btnEliminar);

            // Este eventos se accionará cuando presiones los botones respectivamente

            btnRegistrar.Click += delegate { StartActivity(typeof(RegistroActivity)); };

            btnBuscar.Click += BtnBuscar_Click;
            btnEliminar.Click += BtnEliminar_Click;


            //Listar al inicio de la App
            await GetPersonas();

        }


        private async Task GetPersonas()
        {
            var dato = await controller.GetPersonas();

            ArrayAdapter array = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleExpandableListItem1, dato.Select(m => m.nombre).ToArray());

            listPersonas.Adapter = array;
        }

        private async void BtnBuscar_Click(object sender, EventArgs e)
        {

            string mensaje = null;

            if (!string.IsNullOrEmpty(txtID.Text))
            {
                var id = int.Parse(txtID.Text);

                persona = await controller.GetPersonasId(id);

                mensaje = (persona != null) ? "Hola: " + persona.nombre : " Dato no encontrado";

                Toast.MakeText(this, mensaje, ToastLength.Long).Show();
            }


        }

        private async void BtnEliminar_Click(object sender, EventArgs e)
        {
            string mensaje = null;

            if (!string.IsNullOrEmpty(txtID.Text))
            {
                var id = int.Parse(txtID.Text);

                persona = await controller.DeletePersona(id);

                mensaje = (persona != null) ? persona.nombre + " ha sido eliminado" : "Error al eliminar";

                Toast.MakeText(this, mensaje, ToastLength.Long).Show();

                await GetPersonas();
            }
        }


    }

}

