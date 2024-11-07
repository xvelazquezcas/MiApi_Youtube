<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MiApi_Youtube._Default" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Busca el canal de tu interes</h2>
     <link rel="stylesheet" type="text/css" href="Css\StyleSheet.css">

       <!-- Búsqueda de canales -->
    <div>
        <label for="terminoBusqueda">Buscar canales:</label>
        <asp:TextBox ID="txtTerminoBusqueda" runat="server" />
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
    </div>

    <!-- Resultados de la búsqueda de canales -->
    <div>
        <h3>Resultados de la búsqueda:</h3>
        <asp:GridView ID="gvResultados" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvResultados_SelectedIndexChanged">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img src='<%# Eval("UrlMiniatura") %>' alt="Imagen del Canal" width="120px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Titulo" HeaderText="Nombre del Canal" />
                <asp:BoundField DataField="IdCanal" HeaderText="ID del Canal" />
                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="Ver Videos" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Resultados de los videos del canal -->
    <div>
        <h3>---Videos del canal---</h3>
        <asp:GridView ID="gvVideos" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvVideos_SelectedIndexChanged">
            <Columns>
                <asp:TemplateField HeaderText="Miniatura del Video">
                    <ItemTemplate>
                        <img src='<%# Eval("UrlMiniaturaVideo") %>' alt="Imagen del Video" width="120px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TituloVideo" HeaderText="Título del Video" />
                <asp:BoundField DataField="IdVideo" HeaderText="ID del Video" />
                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="Ver Comentarios" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Comentarios del video -->
    <div>
        <h3>---Comentarios del video seleccionado---</h3>
        <asp:GridView ID="gvComentarios" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Autor" HeaderText="Autor" />
                <asp:BoundField DataField="Texto" HeaderText="Comentario" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
