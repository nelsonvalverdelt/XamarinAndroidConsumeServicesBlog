using System;
using Android.App;
using Android.OS;
using Android.Widget;

//Importamos
using DemoApp.portable;

namespace DemoApp.droid
{
    [Activity(Label = "RegistroActivity")]
 
    public class RegistroActivity : Activity
    {
        EditText txtNombre, txtApellido, txtEdad, txtEstatura;
        Spinner spSexo;
        Button btnRegistrarPersona;
        string sexoSelected;

        Services controller = new Services();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //"Seteamos" nuestra vista Registro.axml
            SetContentView(Resource.Layout.Registro);

            // Arreglo para el spinner "Sexo"
            string[] dato = { "F", "M" };

            //Tomamos el id de nuestros layout y hacemos un casting con nuestros objetos respectivamente

            txtNombre = FindViewById<EditText>(Resource.Id.txtNombre);
            txtApellido = FindViewById<EditText>(Resource.Id.txtApellido);
            txtEdad = FindViewById<EditText>(Resource.Id.txtEdad);
            txtEstatura = FindViewById<EditText>(Resource.Id.txtEstatura);

            spSexo = FindViewById<Spinner>(Resource.Id.spSexo);

            btnRegistrarPersona = FindViewById<Button>(Resource.Id.btnRegistrarPersona);

            //Adaptador que contiene un arreglo dato 
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, dato);
            spSexo.Adapter = adapter;

            //Evento para registrar Personas
            btnRegistrarPersona.Click += BtnRegistrarPersona_Click;

            //Evento Spinner para obtener el item seleccionado
            spSexo.ItemSelected += SpSexo_ItemSelected;

        }

        private void SpSexo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            sexoSelected = spSexo.GetItemAtPosition(e.Position).ToString();
        }

        private async void BtnRegistrarPersona_Click(object sender, EventArgs e)
        {
            string respuesta;

            if (!string.IsNullOrEmpty(txtNombre.Text) || !string.IsNullOrEmpty(txtApellido.Text) || !string.IsNullOrEmpty(txtEdad.Text) || !string.IsNullOrEmpty(txtEstatura.Text))
            {
                Persona persona = new Persona()
                {
                    nombre = txtNombre.Text,
                    apellido = txtApellido.Text,
                    edad = int.Parse(txtEdad.Text),
                    estatura = double.Parse(txtEstatura.Text),
                    sexo = sexoSelected
                };

                respuesta = await controller.PostPersona(persona);

                //Llamamos a nuestro activity Principal (La vieja confiable)
                StartActivity(typeof(MainActivity));

            }
            else
            {
                respuesta = "completar campos vacios";
            }

            Toast.MakeText(this, respuesta, ToastLength.Long).Show();

        }
    }

}