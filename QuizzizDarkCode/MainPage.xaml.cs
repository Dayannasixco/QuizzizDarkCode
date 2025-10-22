namespace QuizzizDarkCode
{
    public partial class MainPage : ContentPage
    {
        class Pregunta
        {
            public string Texto { get; set; }
            public List<string> Opciones { get; set; }
            public int IndiceCorrecto { get; set; }
        }

        List<Pregunta> preguntas;
        int indicePregunta = 0;
        int puntaje = 0;
        bool preguntaRespondida = false;

        public MainPage()
        {
            InitializeComponent();
            InicializarPreguntas();
            CargarPregunta();
        }

        void InicializarPreguntas()
        {
            preguntas = new List<Pregunta>
        {
            new Pregunta { Texto = "¿Cuál es el único planeta del Sistema Solar que gira en sentido horario (rotación retrógrada)?", Opciones = new List<string>{ "Tierra", "Venus", "Marte", "Júpiter"}, IndiceCorrecto = 1 },
            new Pregunta { Texto = "¿Qué tipo de enlace químico implica la transferencia completa de electrones de un átomo a otro?", Opciones = new List<string>{ "Enlace covalente", "Enlace metálico", " Enlace iónico", "Puente de hidrógeno"}, IndiceCorrecto = 2 },
            new Pregunta { Texto = "En genética, ¿cuál de las siguientes estructuras se encuentra en el núcleo de la célula?", Opciones = new List<string>{ "Ribosoma", "Cromosoma", "Mitocondria", "Cloroplasto"}, IndiceCorrecto = 1 },
            new Pregunta { Texto = "¿Qué ley establece que “la presión de un gas es inversamente proporcional a su volumen” (a temperatura constante)?", Opciones = new List<string>{ "Ley de Charles", " Ley de Boyle ", "Ley de Avogadro", " Ley de Dalton"}, IndiceCorrecto = 1 },
            new Pregunta { Texto = "¿Cuál de los siguientes organismos es un productor en una cadena alimentaria?", Opciones = new List<string>{ "León", "Hongo", "Pasto", "Tiburón"}, IndiceCorrecto = 2 },
        };
        }

        void CargarPregunta()
        {
            preguntaRespondida = false;
            btnSiguiente.IsEnabled = false;
            OpcionesStack.Children.Clear();

            var preguntaActual = preguntas[indicePregunta];

            // Actualizar contador y pregunta
            lblContador.Text = $"Pregunta {indicePregunta + 1} de {preguntas.Count}";
            lblPregunta.Text = preguntaActual.Texto;

            // Crear botones de opciones con mejor diseño
            for (int i = 0; i < preguntaActual.Opciones.Count; i++)
            {
                var frame = new Frame
                {
                    BackgroundColor = Colors.White,
                    BorderColor = Colors.LightGray,
                    CornerRadius = 12,
                    Padding = 0,
                    Margin = new Thickness(5, 0)
                };

                var btn = new Button
                {
                    Text = preguntaActual.Opciones[i],
                    BackgroundColor = Colors.Transparent,
                    TextColor = Colors.Black,
                    FontSize = 16,
                    CornerRadius = 12,
                    Padding = new Thickness(15, 12),
                    HorizontalOptions = LayoutOptions.Fill
                };

                int indice = i;
                btn.Clicked += (s, e) => OpcionSeleccionada(frame, btn, indice);

                frame.Content = btn;
                OpcionesStack.Children.Add(frame);
            }
        }

        void OpcionSeleccionada(Frame frameSeleccionado, Button btnSeleccionado, int indice)
        {
            if (preguntaRespondida)
                return;

            preguntaRespondida = true;
            btnSiguiente.IsEnabled = true;

            var preguntaActual = preguntas[indicePregunta];

            // Deshabilitar todos los botones
            foreach (Frame frame in OpcionesStack.Children)
            {
                if (frame.Content is Button btn)
                    btn.IsEnabled = false;
            }

            if (indice == preguntaActual.IndiceCorrecto)
            {
                // Respuesta correcta
                frameSeleccionado.BackgroundColor = Color.FromArgb("#4CAF50");
                btnSeleccionado.TextColor = Colors.White;
                puntaje += 2;
            }
            else
            {
                // Respuesta incorrecta
                frameSeleccionado.BackgroundColor = Color.FromArgb("#F44336");
                btnSeleccionado.TextColor = Colors.White;

                // Mostrar la respuesta correcta
                var frameCorreto = OpcionesStack.Children[preguntaActual.IndiceCorrecto] as Frame;
                if (frameCorreto?.Content is Button btnCorrecto)
                {
                    frameCorreto.BackgroundColor = Color.FromArgb("#4CAF50");
                    btnCorrecto.TextColor = Colors.White;
                }
            }
        }

        private void btnSiguiente_Clicked_1(object sender, EventArgs e)
        {
            indicePregunta++;

            if (indicePregunta < preguntas.Count)
            {
                CargarPregunta();
            }
            else
            {
                MostrarResultados();
            }
        }

        void MostrarResultados()
        {
            // Ocultar elementos del quiz
            lblContador.IsVisible = false;
            lblPregunta.IsVisible = false;
            OpcionesStack.IsVisible = false;
            btnSiguiente.IsVisible = false;

            // Mostrar resultados
            ResultadosStack.IsVisible = true;

            int maxPuntaje = preguntas.Count * 2;
            double porcentaje = (double)puntaje / maxPuntaje * 100;

            // Actualizar labels
            lblResultados.Text = $"Puntaje: {puntaje} / {maxPuntaje}";
            lblPorcentaje.Text = $"{porcentaje:F0}% de respuestas correctas";

            // Crear barra de progreso mejorada
            CrearBarraProgreso(porcentaje);

            // Mensaje personalizado según el puntaje
            string mensaje = porcentaje switch
            {
                >= 80 => "🎉 ¡Increíble! Dominas perfectamente estos temas.",
                >= 60 => "👏 ¡Muy bien! Tienes buenos conocimientos.",
                >= 40 => "💪 ¡Bien! Con un poco más de estudio serás genial.",
                _ => "📚 ¡No te rindas! La práctica hace al maestro."
            };
            lblMensaje.Text = mensaje;
        }

        void CrearBarraProgreso(double porcentaje)
        {
            GraficaContainer.Children.Clear();

            // Determinar color según el porcentaje
            Color colorBarra;
            string textoRendimiento;

            if (porcentaje >= 80)
            {
                colorBarra = Color.FromArgb("#4CAF50"); // Verde - Excelente
                textoRendimiento = "¡Excelente!";
            }
            else if (porcentaje >= 60)
            {
                colorBarra = Color.FromArgb("#2196F3"); // Azul - Muy bien
                textoRendimiento = "¡Muy bien!";
            }
            else if (porcentaje >= 40)
            {
                colorBarra = Color.FromArgb("#FF9800"); // Naranja - Regular
                textoRendimiento = "Regular";
            }
            else
            {
                colorBarra = Color.FromArgb("#F44336"); // Rojo - Necesita mejorar
                textoRendimiento = "Puedes mejorar";
            }

            // Crear contenedor con información del color
            var infoStack = new VerticalStackLayout { Spacing = 8 };

            // Label que muestra el rendimiento
            var lblRendimiento = new Label
            {
                Text = textoRendimiento,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = colorBarra
            };

            // Crear barra de fondo
            var barraFondo = new BoxView
            {
                BackgroundColor = Color.FromArgb("#E0E0E0"),
                HeightRequest = 30,
                CornerRadius = 15,
                HorizontalOptions = LayoutOptions.Fill
            };

            // Crear barra de progreso
            var barraProgreso = new BoxView
            {
                BackgroundColor = colorBarra,
                HeightRequest = 30,
                CornerRadius = 15,
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = 0 // Se animará
            };

            // Label con el porcentaje sobre la barra
            var lblPorcentajeBarra = new Label
            {
                Text = $"{porcentaje:F0}%",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // Crear grid para superponer las barras y el texto
            var grid = new Grid();
            grid.Children.Add(barraFondo);
            grid.Children.Add(barraProgreso);
            grid.Children.Add(lblPorcentajeBarra);

            infoStack.Children.Add(lblRendimiento);
            infoStack.Children.Add(grid);

            GraficaContainer.Children.Add(infoStack);

            // Animar la barra de progreso
            AnimarBarraProgreso(barraProgreso, porcentaje);
        }

        async void AnimarBarraProgreso(BoxView barra, double porcentajeObjetivo)
        {
            double anchoMaximo = 280; // Ancho aproximado del contenedor
            double anchoObjetivo = (anchoMaximo * porcentajeObjetivo) / 100;

            // Asegurar que siempre haya algo visible, mínimo 10 píxeles
            if (anchoObjetivo < 10 && porcentajeObjetivo > 0)
                anchoObjetivo = 10;

            await barra.LayoutTo(new Rect(0, 0, anchoObjetivo, 30), 1500, Easing.CubicOut);
        }

        private void btnReiniciar_Clicked(object sender, EventArgs e)
        {
            // Reiniciar variables
            indicePregunta = 0;
            puntaje = 0;
            preguntaRespondida = false;

            // Mostrar elementos del quiz
            lblContador.IsVisible = true;
            lblPregunta.IsVisible = true;
            OpcionesStack.IsVisible = true;
            btnSiguiente.IsVisible = true;

            // Ocultar resultados
            ResultadosStack.IsVisible = false;

            // Cargar primera pregunta
            CargarPregunta();
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Inicio());
        }
    }

}
