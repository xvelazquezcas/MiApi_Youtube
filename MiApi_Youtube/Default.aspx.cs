using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiApi_Youtube
{
    public partial class _Default : Page
    {
        private static string idCanalSeleccionado; //Guarda el ID del canal que se seleccione
        private static string idVideoSeleccionado; //Guarda el ID del video que se seleccione

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // Método de búsquedad de canales
        protected async void btnBuscar_Click(object sender, EventArgs e) //Evento del botón de búsquedad.
        {
            string terminoBusqueda = txtTerminoBusqueda.Text.Trim(); //Obtiene el texto ingresado en el cuadro de búsquedad, elimina espacios en blanco antes y después.
            if (!string.IsNullOrEmpty(terminoBusqueda)) //Si -terminoBusquedad- no esta vacío, llama al método -BuscarCanalesEnYoutube- de manera asíncrona (await)
            {
                await BuscarCanalesEnYouTube(terminoBusqueda);
            }
        }

        // Método para buscar canales en YouTube
        private async Task BuscarCanalesEnYouTube(string terminoBusqueda)
        {
            var servicioYouTube = new YouTubeService(new BaseClientService.Initializer() //Crea una instancia de YoutubeService que es la clase que proporciona la api para interactuar con youtube
            {
                ApiKey = "AIzaSyBrlY6lsUxsd4y68ihDF5G9btx8n086lII", //Apy Key
                ApplicationName = "AppBusquedaCanalesYouTube" //Definicion del nombre de la aplicacion
            });
            //Creamos una solicitud de búsquedad
            var solicitudBusqueda = servicioYouTube.Search.List("snippet"); //Indica que queremos buscar elementos de tipo "snippet" 
            solicitudBusqueda.Q = terminoBusqueda; //Asigna el termino de busquedad que ingresaste a la consulta
            solicitudBusqueda.Type = "channel"; //Indica que estamos buscando canales
            solicitudBusqueda.MaxResults = 10; // Limita los resultados a 10 canales

            var respuestaBusqueda = await solicitudBusqueda.ExecuteAsync(); //Envia la solicitud de búsqueda a youtube y espera de manera asincrona

            var resultados = new List<dynamic>(); //Lista para almacenar resultados de la búsquedad

            foreach (var item in respuestaBusqueda.Items) //Bucle para recorrer cada item en la lista de resultados 
            {
                resultados.Add(new
                {
                    Titulo = item.Snippet.Title,
                    IdCanal = item.Snippet.ChannelId,
                    UrlMiniatura = item.Snippet.Thumbnails.Default__.Url // URL de la miniatura
                });
            }

            gvResultados.DataSource = resultados;
            gvResultados.DataBind(); //Vincula los datos a la interfaz gráfica
        }

        // Evento al seleccionar un canal para mostrar sus videos
        protected async void gvResultados_SelectedIndexChanged(object sender, EventArgs e)
        {
            idCanalSeleccionado = gvResultados.SelectedRow.Cells[2].Text; //Obtiene el ID del canal de la fila seleccionada 
            await CargarVideosDelCanal(idCanalSeleccionado); //Llama de manera asincrona al metodo CargarVideosDelCanal para cargar los videos de ese canal
        }

        // Método para cargar los videos de un canal
        //Método similar a BuscarCanalesEnYoutube, pero en lugar de buscar canales, ahora busca videos de un canal especifico
        private async Task CargarVideosDelCanal(string idCanal)
        {
            var servicioYouTube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBrlY6lsUxsd4y68ihDF5G9btx8n086lII",
                ApplicationName = "AppBusquedaCanalesYouTube"
            });
            //Solicitud de búsqueda, peroa ahora con el ID del canal
            var solicitudBusqueda = servicioYouTube.Search.List("snippet");
            solicitudBusqueda.ChannelId = idCanal;
            solicitudBusqueda.Type = "video";
            solicitudBusqueda.MaxResults = 10;

            var respuestaBusqueda = await solicitudBusqueda.ExecuteAsync();

            var videos = new List<dynamic>();

            foreach (var video in respuestaBusqueda.Items)
            {
                videos.Add(new
                {
                    TituloVideo = video.Snippet.Title,
                    IdVideo = video.Id.VideoId,
                    UrlMiniaturaVideo = video.Snippet.Thumbnails.Default__.Url // URL de la miniatura del video
                });
            }

            gvVideos.DataSource = videos;
            gvVideos.DataBind();
        }

        // Evento al seleccionar un video para mostrar los comentarios
        protected async void gvVideos_SelectedIndexChanged(object sender, EventArgs e)
        {
            idVideoSeleccionado = gvVideos.SelectedRow.Cells[2].Text;
            await CargarComentariosDelVideo(idVideoSeleccionado);
        }

        // Método para cargar los comentarios de un video
        private async Task CargarComentariosDelVideo(string idVideo)
        {
            var servicioYouTube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBrlY6lsUxsd4y68ihDF5G9btx8n086lII",
                ApplicationName = "AppBusquedaCanalesYouTube"
            });

            var solicitudComentarios = servicioYouTube.CommentThreads.List("snippet");
            solicitudComentarios.VideoId = idVideo;
            solicitudComentarios.MaxResults = 10;

            var respuestaComentarios = await solicitudComentarios.ExecuteAsync();

            var comentarios = new List<dynamic>();

            foreach (var comentario in respuestaComentarios.Items)
            {
                comentarios.Add(new
                {
                    Autor = comentario.Snippet.TopLevelComment.Snippet.AuthorDisplayName,
                    Texto = comentario.Snippet.TopLevelComment.Snippet.TextOriginal
                });
            }

            gvComentarios.DataSource = comentarios;
            gvComentarios.DataBind();
        }
    }
}