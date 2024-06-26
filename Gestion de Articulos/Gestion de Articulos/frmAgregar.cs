﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using gestor;

namespace Gestion_de_Articulos
{
    public partial class frm_Agregar : Form
    {
        private Articulo articulo = null;
        public frm_Agregar()
        {
            InitializeComponent();
        }

        public frm_Agregar(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Editar artículo";
        }
        private void frm_Agregar_Load(object sender, EventArgs e)
        {
            gestionMarca marca = new gestionMarca();
            gestionCategoria cat = new gestionCategoria();
            try
            {
                cbo_Marca.DataSource = marca.listar();
                cbo_Marca.ValueMember = "IdMarca";
                cbo_Marca.DisplayMember = "Descripcion";
                cbo_Categoria.DataSource = cat.listar();
                cbo_Categoria.ValueMember = "IdCategoria";
                cbo_Categoria.DisplayMember = "Descripcion";
                if (articulo != null)
                {
                    tbx_Codigo.Text = articulo.Codigo;
                    tbx_Nombre.Text = articulo.Nombre;
                    tbx_Descripcion.Text = articulo.Descripcion;
                    cbo_Marca.SelectedValue = articulo.Marca.IdMarca;
                    cbo_Categoria.SelectedValue = articulo.Categoria.IdCategoria;
                    tbx_Precio.Text = articulo.Precio.ToString();
                    tbx_url.Text = articulo.imagenes[0];
                    cargarImagen(articulo.imagenes[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btn_Guardar_Click(object sender, EventArgs e)
        {
            gestionArticulo gestion = new gestionArticulo();
            try
            {
                if (articulo == null)
                    articulo = new Articulo();
                articulo.Codigo = tbx_Codigo.Text;
                articulo.Nombre = tbx_Nombre.Text;
                articulo.Descripcion = tbx_Descripcion.Text;
                articulo.Marca = (Marca)cbo_Marca.SelectedItem;
                articulo.Categoria = (Categoria)cbo_Categoria.SelectedItem;
                articulo.Precio = decimal.Parse(tbx_Precio.Text);
                List<string> urlImagenes = tbx_url.Text.Split(',').ToList();
                foreach (string palabra in urlImagenes)
                {
                    if (palabra.Contains(","))
                    {
                        urlImagenes.Remove(palabra);
                    }
                }
                articulo.imagenes = urlImagenes;

                if (articulo.Id != 0)
                {
                    gestion.modificar(articulo);
                    MessageBox.Show("El artículo se editó exitosamente");
                }
                else
                {
                    gestion.agregar(articulo);
                    MessageBox.Show("El artículo se guardó exitosamente");
                }
                Close();
            }
            catch (Exception )
            {
                MessageBox.Show("Por favor complete los campos correctamente");

                lbl_Ex_Precio.Text = "Solo numeros";
                lbl_Ex_Descripcion.Text = "Maximo 50 caracteres";
            }
        }
        private void btn_Cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbx_url_Leave(object sender, EventArgs e)
        {
            cargarImagen(tbx_url.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbx_nuevo_art.Load(imagen);
            }
            catch (Exception ex)
            {
                pbx_nuevo_art.Load("https://t4.ftcdn.net/jpg/05/17/53/57/360_F_517535712_q7f9QC9X6TQxWi6xYZZbMmw5cnLMr279.jpg");
            }
        }
    }
}
