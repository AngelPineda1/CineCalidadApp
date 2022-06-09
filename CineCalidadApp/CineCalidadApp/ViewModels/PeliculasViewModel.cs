using CineCalidadApp.Models;
using CineCalidadApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CineCalidadApp.ViewModels
{
    public class PeliculasViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<Pelicula> Peliculas { get; set; } = new ObservableCollection<Pelicula>();
        AgregarPeliculaView vistaAgregar;
        EditarPeliculaView vistaEditar;
        DetallesPeliculaView vistaDetalles;

        public event PropertyChangedEventHandler PropertyChanged;

        public Pelicula Pelicula { get; set; }
        public string Error { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand MostrarDetallesCommand { get; set; }
        public ICommand GuardarCommand { get; set; }

        public PeliculasViewModel()
        {
            Deserializar();
            CambiarVistaCommand = new Command<string>(CambiarVista);
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command<Pelicula>(Editar);
            EliminarCommand = new Command<Pelicula>(Eliminar);
            MostrarDetallesCommand = new Command<Pelicula>(MostrarDetalles);
            GuardarCommand = new Command(Guardar);

        }

        void Serializar()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "peliculas.json";
            File.WriteAllText(file, JsonConvert.SerializeObject(Peliculas));
        }
        void Deserializar()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "peliculas.json";
            if (File.Exists(file))
            {
                Peliculas = JsonConvert.DeserializeObject<ObservableCollection<Pelicula>>(File.ReadAllText(file));
            }
        }
        private void CambiarVista(string vista)
        {
            if (vista == "Agregar")
            {
                
                vistaAgregar = new AgregarPeliculaView();
                Application.Current.MainPage.Navigation.PushAsync(vistaAgregar);
                Pelicula = new Pelicula();
            }
            else if (vista == "Ver")
            {
                Application.Current.MainPage.Navigation.PopToRootAsync();
            }
        }
        private void Agregar()
        {
            Error = "";
            if (Pelicula != null)
            {
                
                if (string.IsNullOrWhiteSpace(Pelicula.Titulo))
                {
                    Error = "Debe ingresar un titulo";
                   
                }
                if (string.IsNullOrWhiteSpace(Pelicula.Genero))
                {
                    Error = "Debe ingresar un genero";
                }
                if (string.IsNullOrWhiteSpace(Pelicula.Duracion))
                {
                    Error = "Ingrese la duracion de la pelicula";
                }
                if (!Uri.TryCreate(Pelicula.Imagen, UriKind.Absolute, out var uri))
                {
                    Error = "Escriba una URL de imagen valida";

                }
                if (Error == "")
                {
                    Peliculas.Add(Pelicula);
                    Serializar();
                    CambiarVista("Ver");
                }
                Change();
            }
            
        }
        int indice;
        private void Editar(Pelicula p)
        {
            indice = Peliculas.IndexOf(p);

            Pelicula= new Pelicula
            {
                Titulo = p.Titulo,
                Genero = p.Genero,
                Duracion = p.Duracion,
                Sala = p.Sala
            };

            vistaEditar = new EditarPeliculaView()
            {
                BindingContext = this
            };

            App.Current.MainPage.Navigation.PushAsync(vistaEditar);
        }
        private void Eliminar(Pelicula p)
        {
            if (p != null)
            {
                Peliculas.Remove(p);
                Serializar();
            }
        }

        private void Guardar()
        {
            Peliculas[indice] = Pelicula;
            Serializar();
            App.Current.MainPage.Navigation.PopToRootAsync();
        }
        private void MostrarDetalles(Pelicula obj)
        {

            vistaDetalles = new DetallesPeliculaView()
            {
                BindingContext = obj
            };
            App.Current.MainPage.Navigation.PushAsync(vistaDetalles);
        }
        private void Change()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
