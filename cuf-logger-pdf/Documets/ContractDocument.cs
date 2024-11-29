using cuf_admision_db_map.AdmisionDb.Tables;
using cuf_admision_domain.Entities.Application;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using Document = iTextSharp.text.Document;
using Rectangle = iTextSharp.text.Rectangle;

namespace cuf_admision_pdf.Documets
{
    /*
     PAis -> codigo to name
     */
    public class ContractDocument
    {
        public string documentName { get; set; }
        private readonly ApplicantInfo _data;


        
        Font fTitle;//new Font(Font.NORMAL, 18);
        Font fSs;
        Font fTBold;
        Font fTnormal;
        private readonly bool signed;
        public ContractDocument(string filePath, ApplicantInfo data, bool signed = false)
        {
            _data = data;
            this.signed = signed;
            System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);

            string startupPath = Environment.CurrentDirectory;
            BaseFont customfont = BaseFont.CreateFont(
                startupPath+@"/Roboto/Roboto-Regular.ttf",
                BaseFont.CP1252, BaseFont.EMBEDDED);
            BaseFont customfontBold = BaseFont.CreateFont(
                startupPath + @"/Roboto/Roboto-Bold.ttf",
                BaseFont.CP1252, BaseFont.EMBEDDED);

            documentName = filePath;
            //Font fTitle = FontFactory.GetFont("Arial", 18, BaseColor.BLACK);//new Font(Font.NORMAL, 18);
            fTitle = new Font(customfontBold, 18, Font.BOLD);
            fSs = new Font(customfontBold, 10, Font.BOLD);
            fTBold = new Font(customfontBold, 10, Font.BOLD);
            fTnormal = new Font(customfont, 10, Font.NORMAL);
        }
     
        public void Generate()
        {

            FileStream fs = new FileStream(documentName, FileMode.Create, FileAccess.Write, FileShare.None);
            Rectangle rec = new Rectangle(PageSize.A4);
            Document doc = new Document(rec);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            string programMonthly = string.Format("{0:#.00}", ((_data.application.program.creditCost * _data.application.program.credits) / (_data.application.program.cycles * 3)));
            string regionInfo = _data.country.code == "US"
                ? $"Estado {_data.state?.name} Código Zip {_data.personal.postalCode} País {_data.country.name.ToUpper()}"
                : $"Estado           Código Zip             País {_data.country.name.ToUpper()}";

            doc.Open();
            doc.Add(new Paragraph("CONTINENTAL UNIVERSITY OF FLORIDA", fTitle)
                { Alignment = Element.ALIGN_CENTER});
            doc.Add(new Paragraph(
                "5201 Blue Lagoon\nDrive, 8th Floor Miami, FL 33126\n(786) 220-2888\n", fSs)
                { Alignment = Element.ALIGN_CENTER });

            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("CONTRATO DE MATRÍCULA", fTitle)
                { Alignment = Element.ALIGN_CENTER });
            doc.Add(new Paragraph("\n\n"));

            doc.Add(new Paragraph("INFORMACIÓN DEL ESTUDIANTE:", fTBold));
            doc.Add(new Paragraph($"Nombre del estudiante: {_data.personal.name.ToUpper()} {_data.personal.surname.ToUpper()} {_data.personal.mothername.ToUpper()}", fTnormal));
            doc.Add(new Paragraph($"Tipo de documento: {_data.document.name.ToUpper()} Número {_data.personal.documentNumber.ToUpper()}", fTnormal));
            doc.Add(new Paragraph("Emitido por:", fTnormal));
            doc.Add(new Paragraph($"Fecha de nacimiento {_data.personal.birthdate.ToString("MM-dd-yyyy")} Nacionalidad {_data.nationality.name.ToUpper()}", fTnormal));
            doc.Add(new Paragraph($"Dirección {_data.personal.address.ToUpper()} Ciudad {_data.personal.birthplace.Split('/')[2].ToUpper()}", fTnormal));
            // is country = US => Show Estando and codigo Zip
            doc.Add(new Paragraph(regionInfo, fTnormal));
            doc.Add(new Paragraph($"Email {_data.personal.email} Número telefónico {_data.personal.celphoneNumber}", fTnormal));
            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("Por la presente, presento mi solicitud " +
                "de MATRÍCULA a CONTINENTAL UNIVERSITY OF FLORIDA, en " +
                "adelante la \"Universidad\", para el programa que se " +
                "detalla a continuación. Confirmo que el programa me" +
                " ha sido explicado en su totalidad y acepto todos los" +
                " términos y condiciones enumerados en este Contrato de" +
                " Matrícula, en adelante el \"CONTRATO\", así como a " +
                "acatar las normas y políticas de la Universidad.\n", fTnormal));

            /*PdfPTable academicTable = new PdfPTable(new float[] { 50f, 50f });
            academicTable.DefaultCell.Border = Rectangle.NO_BORDER;
            //PdfPCell _cell = new PdfPCell(new Paragraph("Grado:", fTBold));
            academicTable.AddCell(new PdfPCell(new Paragraph("Grado:", fTBold)) { Border = Rectangle.NO_BORDER });
            academicTable.AddCell(new PdfPCell(new Paragraph($"{_data.application.level.requiredGrade.ToUpper()}", fTnormal)) { Border = Rectangle.NO_BORDER });

            academicTable.AddCell(new PdfPCell(new Paragraph("Nombre del programa:", fTBold)) { Border = Rectangle.NO_BORDER });
            academicTable.AddCell(new PdfPCell(new Paragraph(_data.application.program.degree.ToUpper(), fTnormal)) { Border = Rectangle.NO_BORDER });

            academicTable.AddCell(new PdfPCell(new Paragraph("Fecha de inicio:", fTBold)) { Border = Rectangle.NO_BORDER });
            academicTable.AddCell(new PdfPCell(new Paragraph(_data.application.termDates.startAt.ToUpper(), fTnormal)) { Border = Rectangle.NO_BORDER });

            academicTable.AddCell(new PdfPCell(new Paragraph("Fecha de graduación prevista:", fTBold)) { Border = Rectangle.NO_BORDER });
            academicTable.AddCell(new PdfPCell(new Paragraph(_data.application.termDates.endAt.ToUpper(), fTnormal)) { Border = Rectangle.NO_BORDER });

            doc.Add(new Paragraph("\n\n"));
            doc.Add(academicTable);
            */
            doc.Add(new Paragraph("\n\n"));

            doc.Add(new Paragraph($"Este programa requiere que {_data.application.program.credits} " +
                $"horas-crédito de instrucción se tomen en {_data.application.program.cycles} semestres " +
                "académicos. Una vez completado con éxito el programa, " +
                "el estudiante recibirá el grado académico de " +
                $"{_data.application.program.degree}.\n", fTnormal));

            doc.Add(new Paragraph("\n\n"));

            doc.Add(new Paragraph("COSTOS Y TARIFAS:", fTBold));
            doc.Add(new Paragraph(
                "Estoy de acuerdo en pagar los Costos y Tarifas por " +
                "conceptos de Inscripción, Colegiatura y Tecnología que " +
                "se detallan a continuación. Todos los pagos son " +
                "reembolsables sujetos a las condiciones señaladas en la " +
                "Política de Reembolso que se encuentra en este documento."
                , fTnormal));
            doc.Add(new Paragraph(
                $"La Cuota de Matrícula es de $. {string.Format("{0:#.00}", _data.application.payments.enr)}. " +
                "La Cuota de Colegiatura del programa elegido es de " +
                $"$ {string.Format("{0:#.00}", _data.application.program.creditCost)} por hora-crédito. El costo total del programa, " +
                "como se detalla a continuación, se reducirá " +
                "proporcionalmente para reflejar cualquier hora-crédito " +
                "aceptada por la Universidad y transferida desde otra " +
                "institución educativa.", fTnormal));

            doc.Add(new Paragraph("\n\n"));

            PdfPTable paymentTable = new PdfPTable(new float[] { 35f, 65f });
            paymentTable.DefaultCell.Border = Rectangle.NO_BORDER;
            //PdfPCell _cell = new PdfPCell(new Paragraph("Grado:", fTBold));
            paymentTable.AddCell(new PdfPCell(new Paragraph("Costo total del programa:", fTnormal)) { Border = Rectangle.NO_BORDER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"US$ {_data.application.payments.program}", fTnormal)) { Border = Rectangle.NO_BORDER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Reducción del pago inicial:", fTnormal)) { Border = Rectangle.NO_BORDER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"US$ ", fTnormal)) { Border = Rectangle.NO_BORDER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Importe a financiar:", fTnormal)) { Border = Rectangle.NO_BORDER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"US$ ", fTnormal)) { Border = Rectangle.NO_BORDER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Pago mensual:", fTnormal)) { Border = Rectangle.NO_BORDER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"US$ {string.Format("{0:#.00}", _data.application.payments.c00)}  A pagar el día 1 de cada mes", fTnormal)) { Border = Rectangle.NO_BORDER });

            doc.Add(new Paragraph("\n\n"));
            doc.Add(paymentTable);
            doc.Add(new Paragraph("\n\n"));

            doc.Add(new Paragraph(
                "También estoy de acuerdo en pagar una Tarifa de Tecnología de " +
                $"$ {string.Format("{0:#.00}", _data.application.payments.tfee)} por cada semestre académico en el que me inscriba. " +
                "Esta tarifa cubre el costo de los materiales de instrucción" +
                " en las plataformas institucionales. Los libros de texto se" +
                " proporcionan en línea sin cargos adicionales. Un programa" +
                $" de {_data.application.level.name} requiere, en promedio, que usted se inscriba" +
                $" {_data.application.program.cycles} semestres académicos lo que resultaría en un costo" +
                $" promedio por Tarifas de Tecnología de $ {string.Format("{0:#.00}", _data.application.payments.tfee)}. " +
                "Sin embargo, el número total de semestres académicos en los" +
                " que usted participe estarán sujetos al número de clases" +
                " inscritas en cada semestre, cursos repetidos y" +
                " horas-crédito transferidas.\n", fTnormal));

            doc.Add(new Paragraph("\n\n"));

            doc.Add(new Paragraph("CÁLCULO DE LA CUOTA DE COLEGIATURA:", fTBold));
            doc.Add(new Paragraph(
                "Las Cuotas de Colegiatura de nuestros programas son " +
                "idénticas para todos los estudiantes y se calculan por " +
                "hora-crédito. Las cuotas totales varían en función de los" +
                " requisitos de horas-crédito del programa. No se distingue" +
                " entre estudiantes a tiempo completo y a tiempo parcial a" +
                " la hora de calcular las cuotas del programa y los pagos" +
                " mensuales. Los programas son 100% online y están disponibles" +
                " las 24 horas del día durante cada semestre académico.\n",
                fTnormal));

            doc.Add(new Paragraph("\n\n"));

            doc.Add(new Paragraph("FORMAS DE PAGO:", fTBold));
            List listPayment = new List(20f);
            listPayment.SetListSymbol("\u2022");
            listPayment.Add(new ListItem(
                $"La Cuota de Matrícula de $ {string.Format("{0:#.00}", _data.application.payments.enr)} se debe pagar en el " +
                "momento de la firma de este CONTRATO.", fTnormal));
            listPayment.Add(new ListItem(
                "La Tarifa de Tecnología aplicable se paga al inicio de " +
                "cada semestre académico en el que el/la estudiante se " +
                "inscriba en las clases. Esta tarifa se incluirá en el " +
                "primer pago.", fTnormal));
            listPayment.Add(new ListItem(
                "La Cuota de Colegiatura de cada semestre académico se " +
                "paga en cuatro cuotas. La primera al inicio del semestre " +
                "académico y las tres cuotas restantes el primer día de los" +
                " meses siguientes.", fTnormal));
            doc.Add(listPayment);


            doc.Add(new Paragraph("\n\n"));

            doc.Add(PaymentPlanTable(_data));

            doc.Add(new Paragraph("\n\n"));



            doc.Add(new Paragraph(
                "Todos los costos de los programas son los que se reflejan " +
                "en este documento. No hay cargos adicionales, cargos por" +
                " intereses o cargos por servicios relacionados con estos" +
                " programas. Este contrato no se venderá ni se transferirá a" +
                " un tercero. El costo del crédito está incluido en el costo" +
                " del servicio. No hay penalización por pagos anticipados."
                , fTnormal));

            doc.NewPage();
            doc.Add(new Paragraph("OBLIGACIONES FINANCIERAS:", fTBold));

            doc.Add(new Paragraph(
                "La Cuota de Colegiatura debe ser pagada a tiempo de acuerdo" +
                " con los términos de este CONTRATO. En caso de " +
                "circunstancias atenuantes, el estudiante debe consultar con" +
                " la Oficina de Admisiones.", fTnormal));

            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("HORARIO DE CLASES:", fTBold));

            doc.Add(new Paragraph(
                "La Universidad ofrece cursos 100% online a través de clases" +
                " asíncronas disponibles en su plataforma institucional. " +
                "Los estudiantes deben participar en los foros en línea, " +
                "completar las tareas o deberes, presentar los exámenes en" +
                " fechas fijas, al menos semanalmente, como se especifica en " +
                "el programa de cada curso. Los estudiantes pueden completar " +
                "toda la participación asíncrona en el horario que deseen " +
                "dentro de los límites establecidos en el calendario " +
                "académico y del curso. Además, los estudiantes pueden " +
                "participar en sesiones sincrónicas no obligatorias."
                , fTnormal));


            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("NOTA ESPECIAL:", fTBold));
            doc.Add(new Paragraph(
                "La UNIVERSIDAD se reserva el derecho de modificar los " +
                "cursos de estudio, el contenido de los cursos, las tasas, " +
                "los requisitos de los programas, los horarios de las clases " +
                "y el calendario académico, así como cualquier otro cambio " +
                "que se considere necesario o conveniente, avisando con " +
                "antelación siempre que sea posible.\n", fTnormal));

            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("EXPULSIÓN DE ESTUDIANTES:", fTBold));
            doc.Add(new Paragraph(
                "Los estudiantes podrán ser expulsados de la Universidad en " +
                "los siguientes casos:", fTnormal));

            List expulsionList = new List(20f);
            expulsionList.SetListSymbol("\u2022");
            expulsionList.Add(new ListItem(
                "Destruir, sustraer, incitar o alterar información de los " +
                "sistemas o registros oficiales de la Universidad " +
                "directamente o a través de terceros sin autorización " +
                "expresa de la Universidad.", fTnormal));

            expulsionList.Add(new ListItem(
                "Presentar datos o documentos falsos en cualquier " +
                "procedimiento administrativo tramitado ante la " +
                "Universidad.", fTnormal));
            expulsionList.Add(new ListItem(
                "Suplantar la identidad de otra persona o hacerse pasar por" +
                " ella en las evaluaciones o en cualquier otra actividad de" +
                " la Universidad.", fTnormal));
            expulsionList.Add(new ListItem(
                "Las credenciales académicas consideradas para la aceptación" +
                " del estudiante están incompletas o han sido falsificadas " +
                "o alteradas de cualquier manera según lo determine la " +
                "Oficina de Admisiones.", fTnormal));
            expulsionList.Add(new ListItem(
                "Incumplimiento de las políticas de asistencia y conducta."
                , fTnormal));
            expulsionList.Add(new ListItem(
                "No cumplir con los estándares mínimos de rendimiento " +
                "académico. En caso de que el estudiante repruebe un curso " +
                "por cuarta (4ª) vez, será dado de baja definitivamente."
                , fTnormal));
            expulsionList.Add(new ListItem(
                "Morosidad de cuatro (4) meses o más en el pago de la " +
                "matrícula.", fTnormal));
            expulsionList.Add(new ListItem(
                "Comportamiento que atente contra la integridad de las " +
                "personas o el desarrollo de las actividades académicas u " +
                "otras conductas contempladas en el reglamento académico."
                , fTnormal));

            doc.Add(expulsionList);


            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("POLÍTICA DE REEMBOLSO:", fTBold));
            doc.Add(new Paragraph("En el caso de que el/la estudiante sea expulsado o cancele voluntariamente su matrícula o se dé de baja por cualquier motivo, el/la estudiante podrá tener derecho a la devolución del dinero, en base a las normas que se detallan a continuación. El estudiante deberá solicitar el reembolso por escrito enviando a student.services@continentaluniversity.us un correo electrónico dentro de los plazos establecidos en esta política.", fTnormal));
            doc.Add(new Paragraph("El proceso de reembolso tiene una duración máxima de treinta (30) días naturales, en los que el reembolso se realizará según las normas que se detallan a continuación:", fTnormal));

            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("1. Cuota de Matrícula:", fTBold));

            List l1 = new List(false, List.ALPHABETICAL, 20f);
            l1.Add(new ListItem(
                "Si la matrícula es cancelada por la UNIVERSIDAD por cualquiera de los motivos indicados en el apartado de Expulsión de Estudiantes y antes de la finalización del primer semestre académico del estudiante, se devolverá íntegramente la Cuota de Matrícula abonadas.", fTnormal));
            l1.Add(new ListItem(
                "Si la matrícula es cancelada por la UNIVERSIDAD en cualquier momento después de que el estudiante complete su primer semestre académico, entonces NO habrá reembolso de la Cuota de Matrícula.", fTnormal));
            l1.Add(new ListItem(
                "En el caso de que el estudiante notifique a la UNIVERSIDAD la cancelación de su matrícula antes de la medianoche del tercer (3er) día hábil después de haber realizado el pago, el importe abonado se reembolsará en su totalidad.", fTnormal));
            l1.Add(new ListItem(
                "Si el estudiante notifica la finalización de su matrícula en la UNIVERSIDAD después del tercer día hábil siguiente al día en que se pagó la Cuota de Matrícula, entonces NO habrá devolución de la Cuota de Matrícula.\n", fTnormal));
            doc.Add(l1);


            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("2. Cuota de Colegiación:", fTBold));
            List l2 = new List(false, List.ALPHABETICAL, 20f);
            l2.Add(new ListItem(
                "En el caso de que el estudiante se dé de baja o sea retirado del periodo académico dentro de los ocho (8) días siguientes al inicio de este (en adelante \"Añadir/Retirar\"), se procederá a la devolución del importe total pagado por el semestre.", fTnormal));
            l2.Add(new ListItem(
                "Si el estudiante se da de baja o es dado de baja de uno o más cursos antes de que finalice el Periodo de Añadir/Retirar y permanece matriculado en uno o más cursos, el reembolso de la Cuota de Colegiación se limitará al importe pagado por los cursos de los que el estudiante se da de baja o es dado de baja.", fTnormal));
            l2.Add(new ListItem(
                "La Cuota de Colegiación de cada semestre académico se paga en cuatro (4) cuotas. La primera al inicio del semestre académico y las tres (3) cuotas siguientes el primer día de cada mes subsiguiente durante cada semestre académico.", fTnormal));
            l2.Add(new ListItem(
                "Cualquier cancelación notificada después de la finalización del período de Añadir/Retirar no dará lugar a la devolución de la Cuota de Matrícula.", fTnormal));
            doc.Add(l2);

            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("3. Tarifa de Tecnología:", fTBold));
            List l3 = new List(false, List.ALPHABETICAL, 20f);
            l3.Add(new ListItem(
                "La Tarifa de Tecnología se reembolsará en su totalidad cuando el estudiante se dé de baja o sea retirado antes de que finalice el periodo de Añadir/Retirar.", fTnormal));
            l3.Add(new ListItem(
                "La Tarifa de Tecnología no se reembolsará si el estudiante se da de baja de un curso y sigue matriculado en uno o más cursos.", fTnormal));
            l3.Add(new ListItem(
                "Cualquier cancelación notificada después de la finalización del Período de Añadir/Retirar no dará lugar a la devolución de la Tarifa de Tecnología.", fTnormal));
            doc.Add(l3);

            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("4. Libros y/o suministros:", fTBold));
            List l4 = new List(List.ALPHABETICAL, 20f);
            l4.Add(new ListItem(
                "No se cobran por separado los libros de texto y hay tasas de libros de texto reembolsables por la Universidad", fTnormal));
            doc.Add(l4);


            doc.Add(new Paragraph("\n\n"));
            doc.Add(new Paragraph("5.Fecha de terminación:", fTBold));

            List l5 = new List(false, List.ALPHABETICAL, 20f);
            l5.Add(new ListItem(
                "La fecha de finalización será la primera de: (1) el último " +
                "día en que el/la estudiante haya participado en clase, (2) " +
                "la fecha en que la UNIVERSIDAD haya enviado la notificación " +
                "de baja al estudiante, o (3) la fecha en que la UNIVERSIDAD " +
                "reciba la notificación del estudiante cancelando su " +
                "matrícula o dándose de baja en el semestre académico o en " +
                "cualquier curso dentro de un semestre académico.", fTnormal));
            l5.Add(new ListItem(
                "Los reembolsos se efectuarán dentro de los treinta (30) " +
                "días siguientes a la fecha de finalización.", fTnormal));
            doc.Add(l5);
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph(
                "Una vez finalizado el programa con éxito, la UNIVERSIDAD " +
                "anima a todos los egresados a aprovechar los servicios " +
                "ofrecidos por la Oficina de Servicios Estudiantiles " +
                "relacionados con el desarrollo de su carrera. Sin embargo, " +
                "la UNIVERSIDAD no garantiza que el estudiante obtenga un " +
                "empleo.", fTnormal));
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph(
                "HE LEÍDO, COMPRENDIDO Y RECIBIDO UNA COPIA EXACTA DE TODAS " +
                "LAS PÁGINAS DE ESTE CONTRATO Y UNA COPIA DEL CATÁLOGO DE LA" +
                " UNIVERSIDAD. ENTIENDO, Y AL FIRMAR ESTE DOCUMENTO ACEPTO," +
                " ESTAR OBLIGADO A CUMPLIR LAS NORMAS Y REGLAMENTOS " +
                "CONTENIDOS EN ESTE DOCUMENTO Y EN EL CATÁLOGO MIENTRAS ESTÉ" +
                " MATRICULADO EN LA UNIVERSIDAD.", fTnormal));
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph(
                "AL FIRMAR ESTE CONTRATO TODAS LAS PARTES ESTÁN DE ACUERDO Y" +
                " ACEPTAN QUE LAS CONDICIONES DETALLADAS EN ESTE CONTRATO, Y" +
                " QUE LAS POLÍTICAS Y PROCEDIMIENTOS DETALLADOS EN EL " +
                "CATÁLOGO SON VINCULANTES PARA TODAS LAS PARTES.", fTBold));
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph(
                "AVISO AL CONSUMIDOR: NO FIRME ESTE CONTRATO SIN LEERLO O SI" +
                " CONTIENE ESPACIOS EN BLANCO. TIENE DERECHO A RECIBIR UNA" +
                " COPIA EXACTA DEL CONTRATO QUE FIRMA. CONSERVE ESA COPIA " +
                "PARA PROTEGER SUS DERECHOS LEGALES.", fTnormal));
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph(
                "NOTA: Este CONTRATO se ha explicado completamente al " +
                "estudiante en inglés.", fTBold));
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph(
                "NOTA: Este Contrato se tradujo y se le explicó al estudiante" +
                " en español.", fTBold));
            doc.Add(new Paragraph("\n"));

            doc.Add(SigningTable(_data, signed));
            doc.Add(new Paragraph("\n\n"));

            doc.Add(new Paragraph(
                "CONTINENTAL UNIVERSITY OF FLORIDA is licensed by the \n" +
                "Florida Department of Education\n" +
                "Comisión de Educación Independiente (CIE)\n" +
                "325 West Gaines St., Suite 1414, Tallahassee, FL 32399-0400\n" +
                "(850) 245-3200 - (888) 224-6684.\n"
                , fSs)
            { Alignment = Element.ALIGN_CENTER });

            doc.Close();
        }

        public void GenerateAddendum( AddendumModel addendumModel)
        {

            FileStream fs = new FileStream(documentName, FileMode.Create, FileAccess.Write, FileShare.None);
            Rectangle rec = new Rectangle(PageSize.A4);
            Document doc = new Document(rec);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            string programMonthly = string.Format("{0:#.00}", ((_data.application.program.creditCost * _data.application.program.credits) / (_data.application.program.cycles * 3)));
            string regionInfo = _data.country.code == "US"
                ? $"Estado {_data.state?.name} Código Zip {_data.personal.postalCode} País {_data.country.name.ToUpper()}"
                : $"Estado           Código Zip             País {_data.country.name.ToUpper()}";

            doc.Open();
            doc.Add(new Paragraph("ANEXO AL CONTRATO DE MATRÍCULA (PRELIMINAR)", fTBold)
            { Alignment = Element.ALIGN_CENTER });
            doc.Add(new Paragraph("\n\n"));

            doc.Add(new Paragraph("INFORMACIÓN DEL ESTUDIANTE:", fTBold));

            doc.Add(new Paragraph($"Nombre del estudiante: {_data.personal.name.ToUpper()} {_data.personal.surname.ToUpper()} {_data.personal.mothername.ToUpper()}", fTnormal));
            doc.Add(new Paragraph($"Tipo de documento: {_data.document.name.ToUpper()} Número {_data.personal.documentNumber.ToUpper()}", fTnormal));

            doc.Add(new Paragraph($"Nombre de Programa: {_data.application.program.name.ToUpper()}", fTnormal));
            doc.Add(new Paragraph($"Fecha de inicio: {_data.application.termDates.termCode.ToUpper()}", fTnormal));

            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph(
              "Del acuerdo firmado en el contrato de matrícula de Continental University of Florida (CUF) se detalla cláusulas de acuerdo adicionales que se detallan a continuación: ", fTnormal));
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph("Cláusula 1: Beca Lead Campaña", fTBold));
            doc.Add(new Paragraph(
             $"Se le otorgará un {(addendumModel.percentagebeca)} de beca que aplicará al pago de los cargos por colegiatura en base al\r\nprograma académico escogido por el Becario Lead Campaña en CUF.", fTnormal));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("La aplicación de esta Beca modifica los montos detallados en el Contrato de Matrícula y se\r\nconvierte en complemento integral de dicho Contrato, siendo vinculante para todas las partes.", fTnormal));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Duración: Esta beca aplica a lo largo del programa siempre que matricule su ciclo completo en\r\nel semestre.", fTnormal));
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph("Obligaciones:", fTBold));
            doc.Add(new Paragraph("· El Becario debe mantener un Promedio de Calificaciones Académicas óptimas (Progreso Académico Satisfactorio, Posgrado Promedio >= B).", fTnormal));
            doc.Add(new Paragraph("· El Becario deberá cumplir con la programación de pagos establecidos, respetando la política de pagos establecidos, si el Becario cambia a un estado de morosidad este beneficio es anulado automáticamente.", fTnormal));

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Cláusula 2: Cursos de Desarrollo", fTBold));
            doc.Add(new Paragraph("Por la carrera seleccionada, no se requieren Cursos de Desarrollo.", fTnormal));

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Cláusula 3: Traslado", fTBold));
            doc.Add(new Paragraph("El estudiante ha iniciado el programa sin trasladar/convalidar cursos de alguna otra institución.", fTnormal));

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("ANEXO AL CONTRATO DE MATRÍCULA (PRELIMINAR)", fTBold)
            { Alignment = Element.ALIGN_CENTER });
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Monto Total Por Pagar:", fTBold)); 
            PdfPTable paymentTable = new PdfPTable(new float[] { 50f, 25f, 25f });
            paymentTable.DefaultCell.Border = Rectangle.ALIGN_MIDDLE;
            //PdfPCell _cell = new PdfPCell(new Paragraph("Grado:", fTBold));
            paymentTable.AddCell(new PdfPCell(new Paragraph("Concepto", fTBold)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph("Unidad", fTBold)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph("Cantidad", fTBold)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Costo por crédito regular:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph("US$", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{Convert.ToDouble(addendumModel.costcredit).ToString("#,##0.00")}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Cantidad de créditos regular:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Créditos", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{addendumModel.cantcredit}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Tiempo requerido para completar regular:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Meses", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{addendumModel.cantmonth}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("% de beca asignada:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Porcentaje", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{(addendumModel.percentagebeca)}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Costo de crédito con beca:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"US$ ", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{Convert.ToDouble(addendumModel.costbeca).ToString("#,##0.00")}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Cantidad de cursos de Desarrollo :", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Cantidad", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{addendumModel.cantcursosdes}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Cantidad de cursos convalidados:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Cantidad", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{addendumModel.cantcursoscon}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Futuros cursos (Solo doble grado):", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Cantidad", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{addendumModel.cantcursosfut}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Costo Total del Programa):", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"USD", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{Convert.ToDouble(addendumModel.costototalprogram).ToString("#,##0.00")}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });


            doc.Add(new Paragraph("\n"));
            doc.Add(paymentTable);
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Cláusula 4: Disposiciones complementarias", fTBold));
            doc.Add(new Paragraph("Para el caso de la solicitud de obtención de Grado o Título, el estudiante CUF debe cancelar el 100 % de la totalidad del costo del programa elegido.", fTnormal));

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Cláusula 5: Documentación formalización de matrícula", fTBold));
            doc.Add(new Paragraph("El estudiante se compromete a entregar la documentación requerida para formalizar su\r\nproceso de admisión antes de culminar el primer mes de inicio de clase del periodo académico\r\nque actualmente está cursando.", fTnormal));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Se deja constancia expresa que este Anexo forma parte integrante del Contrato de Matrícula,\r\npor lo que vincula a las partes que lo suscriban.", fTnormal));


            doc.Add(new Paragraph("\n\n"));
            doc.Add(SigningTable(_data, signed));
            doc.Add(new Paragraph("\n\n"));

            //doc.Add(new Paragraph(
            //    "CONTINENTAL UNIVERSITY OF FLORIDA is licensed by the \n" +
            //    "Florida Department of Education\n" +
            //    "Comisión de Educación Independiente (CIE)\n" +
            //    "325 West Gaines St., Suite 1414, Tallahassee, FL 32399-0400\n" +
            //    "(850) 245-3200 - (888) 224-6684.\n"
            //    , fSs)
            //{ Alignment = Element.ALIGN_CENTER });

            doc.Close();
        }

        public void GeneratePreAddendum(AddendumModel addendumModel)
        {
            FileStream fs = new FileStream(documentName, FileMode.Create, FileAccess.Write, FileShare.None);
            Rectangle rec = new Rectangle(PageSize.A4);
            Document doc = new Document(rec);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            string programMonthly = string.Format("{0:#.00}", ((_data.application.program.creditCost * _data.application.program.credits) / (_data.application.program.cycles * 3)));
            string regionInfo = _data.country.code == "US"
                ? $"Estado {_data.state?.name} Código Zip {_data.personal.postalCode} País {_data.country.name.ToUpper()}"
                : $"Estado           Código Zip             País {_data.country.name.ToUpper()}";

            doc.Open();
            doc.Add(new Paragraph("PRE - ANEXO AL CONTRATO DE MATRÍCULA (PRELIMINAR)", fTBold)
            { Alignment = Element.ALIGN_CENTER });
            doc.Add(new Paragraph("\n\n"));

            doc.Add(new Paragraph("INFORMACIÓN DEL ESTUDIANTE:", fTBold));

            doc.Add(new Paragraph($"Nombre del estudiante: {_data.personal.name.ToUpper()} {_data.personal.surname.ToUpper()} {_data.personal.mothername.ToUpper()}", fTnormal));
            doc.Add(new Paragraph($"Tipo de documento: {_data.document.name.ToUpper()} Número {_data.personal.documentNumber.ToUpper()}", fTnormal));

            doc.Add(new Paragraph($"Nombre de Programa: {_data.application.program.name.ToUpper()}", fTnormal));
            doc.Add(new Paragraph($"Fecha de inicio: {_data.application.termDates.termCode.ToUpper()}", fTnormal));

            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph(
              "Del acuerdo firmado en el contrato de matrícula de Continental University of Florida (CUF) se detalla cláusulas de acuerdo adicionales que se detallan a continuación: ", fTnormal));
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph("Cláusula 1: Beca Lead Campaña", fTBold));
            doc.Add(new Paragraph(
             $"Se le otorgará un {Convert.ToDouble(addendumModel.percentagebeca).ToString("#,##0.00")}% de beca que aplicará al pago de los cargos por colegiatura en base al\r\nprograma académico escogido por el Becario Lead Campaña en CUF.", fTnormal));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("La aplicación de esta Beca modifica los montos detallados en el Contrato de Matrícula y se\r\nconvierte en complemento integral de dicho Contrato, siendo vinculante para todas las partes.", fTnormal));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Duración: Esta beca aplica a lo largo del programa siempre que matricule su ciclo completo en\r\nel semestre.", fTnormal));
            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph("Obligaciones:", fTBold));
            doc.Add(new Paragraph("· El Becario debe mantener un Promedio de Calificaciones Académicas óptimas (Progreso Académico Satisfactorio, Posgrado Promedio >= B).", fTnormal));
            doc.Add(new Paragraph("· El Becario deberá cumplir con la programación de pagos establecidos, respetando la política de pagos establecidos, si el Becario cambia a un estado de morosidad este beneficio es anulado automáticamente.", fTnormal));

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Cláusula 2: Cursos de Desarrollo", fTBold));
            doc.Add(new Paragraph("Por la carrera seleccionada, no se requieren Cursos de Desarrollo.", fTnormal));

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Cláusula 3: Traslado", fTBold));
            doc.Add(new Paragraph("El estudiante ha iniciado el programa sin trasladar/convalidar cursos de alguna otra institución.", fTnormal));

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("ANEXO AL CONTRATO DE MATRÍCULA (PRELIMINAR)", fTBold)
            { Alignment = Element.ALIGN_CENTER });
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Monto Total Por Pagar:", fTBold)); 
            PdfPTable paymentTable = new PdfPTable(new float[] { 50f, 25f, 25f });
            paymentTable.DefaultCell.Border = Rectangle.ALIGN_MIDDLE;
            //PdfPCell _cell = new PdfPCell(new Paragraph("Grado:", fTBold));
            paymentTable.AddCell(new PdfPCell(new Paragraph("Concepto", fTBold)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph("Unidad", fTBold)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph("Cantidad", fTBold)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Costo por crédito regular:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph("US$", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{Convert.ToDouble(addendumModel.costcredit).ToString("#,##0.00")}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Cantidad de créditos regular:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Créditos", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{addendumModel.cantcredit}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Tiempo requerido para completar regular:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Meses", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{addendumModel.cantmonth}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("% de beca asignada:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Porcentaje", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{Convert.ToDouble(addendumModel.percentagebeca).ToString("#,##0.00")}%", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Costo de crédito con beca:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"US$ ", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{Convert.ToDouble(addendumModel.costbeca).ToString("#,##0.00")}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Cantidad de cursos de Desarrollo :", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Cantidad", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{addendumModel.cantcursosdes}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Cantidad de cursos convalidados:", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Cantidad", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{addendumModel.cantcursoscon}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Futuros cursos (Solo doble grado):", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"Cantidad", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{addendumModel.cantcursosfut}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });

            paymentTable.AddCell(new PdfPCell(new Paragraph("Costo Total del Programa):", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_LEFT });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"USD", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });
            paymentTable.AddCell(new PdfPCell(new Paragraph($"{Convert.ToDouble(addendumModel.costototalprogram).ToString("#,##0.00")}", fTnormal)) { BorderWidth = 1f, HorizontalAlignment = Rectangle.ALIGN_CENTER });


            doc.Add(new Paragraph("\n"));
            doc.Add(paymentTable);
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Cláusula 4: Disposiciones complementarias", fTBold));
            doc.Add(new Paragraph("Para el caso de la solicitud de obtención de Grado o Título, el estudiante CUF debe cancelar el 100 % de la totalidad del costo del programa elegido.", fTnormal));

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Cláusula 5: Documentación formalización de matrícula", fTBold));
            doc.Add(new Paragraph("El estudiante se compromete a entregar la documentación requerida para formalizar su\r\nproceso de admisión antes de culminar el primer mes de inicio de clase del periodo académico\r\nque actualmente está cursando.", fTnormal));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Se deja constancia expresa que este Anexo forma parte integrante del Contrato de Matrícula,\r\npor lo que vincula a las partes que lo suscriban.", fTnormal));


            doc.Add(new Paragraph("\n\n"));
            doc.Add(SigningTable(_data, signed));
            doc.Add(new Paragraph("\n\n"));

            //doc.Add(new Paragraph(
            //    "CONTINENTAL UNIVERSITY OF FLORIDA is licensed by the \n" +
            //    "Florida Department of Education\n" +
            //    "Comisión de Educación Independiente (CIE)\n" +
            //    "325 West Gaines St., Suite 1414, Tallahassee, FL 32399-0400\n" +
            //    "(850) 245-3200 - (888) 224-6684.\n"
            //    , fSs)
            //{ Alignment = Element.ALIGN_CENTER });

            doc.Close();
        }

        private PdfPTable PaymentPlanTable(ApplicantInfo data)
        {
            string programTotal = string.Format("{0:#.00}", (_data.application.program.creditCost * _data.application.program.credits));
            string programMonthly = string.Format("{0:#.00}", ((_data.application.program.creditCost * _data.application.program.credits) / (_data.application.program.cycles * 3)));

            PdfPTable t = new PdfPTable(new float[] { 20f, 20f, 20f, 20f, 20f });

            t.AddCell(new PdfPCell(new Paragraph("Tipo de interés anual", fTnormal)));
            t.AddCell(new PdfPCell(new Paragraph("Costo de financiación", fTnormal)));
            t.AddCell(new PdfPCell(new Paragraph("Importe para financiar", fTnormal)));
            t.AddCell(new PdfPCell(new Paragraph("Pago total", fTnormal)));
            t.AddCell(new PdfPCell(new Paragraph("Valor total", fTnormal)));

            Paragraph p1 = new Paragraph("Tipo de interés anual por hora-crédito\n", fTnormal);
            p1.Add(new Paragraph("0.00%", fTBold));
            t.AddCell(new PdfPCell(p1));

            Paragraph p2 = new Paragraph("Costo por hora-crédito en dólares\n", fTnormal);
            p2.Add(new Paragraph($" {string.Format("{0:#.00}", _data.application.program.creditCost)}", fTBold));
            t.AddCell(new PdfPCell(p2));

            Paragraph p3 = new Paragraph("El importe del crédito a proporcionar a su favor\n", fTnormal);
            p3.Add(new Paragraph("NINGUNO", fTBold));
            t.AddCell(new PdfPCell(p3));

            Paragraph p4 = new Paragraph("Cantidad total pagada al finalizar el plan de pago\n", fTnormal);
            p4.Add(new Paragraph($"$ {string.Format("{0:#.00}", programTotal)}", fTBold));
            t.AddCell(new PdfPCell(p4));
            Paragraph p5 = new Paragraph("Cantidad total pagado incluyendo el pago inicial\n", fTnormal);
            p5.Add(new Paragraph($"$ {string.Format("{0:#.00}", programTotal)}", fTBold));
            t.AddCell(new PdfPCell(p5));

            t.AddCell(new PdfPCell(new Paragraph("PLAN DE PAGO", fTnormal)) { Colspan = 5 });

            t.AddCell(new PdfPCell(new Paragraph("Número de cuotas", fTnormal)));
            t.AddCell(new PdfPCell(new Paragraph("Valor de la cuota", fTnormal)));
            t.AddCell(new PdfPCell(new Paragraph("Plazo de pago", fTnormal)) { Colspan = 3 });

            t.AddCell(new PdfPCell(new Paragraph("4 por semestre", fTBold)));
            t.AddCell(new PdfPCell(new Paragraph($"${string.Format("{0:#.00}", _data.application.payments.c00)} (2 asignaturas)", fTBold)));
            t.AddCell(new PdfPCell(new Paragraph(
                "El primer día del semestre académico y el primer día del " +
                "mes dentro del semestre.", fTnormal))
            { Colspan = 3 });


            return t;
        }
         
        private PdfPTable SigningTable(ApplicantInfo data, bool signed)
        {
            PdfPTable t = new PdfPTable(new float[] { 50f, 50f });
            DateTime now = DateTime.Now;
            bool isGreater18 = yearsDiff(data.personal.birthdate, now) >= 18;
            string signDate = signed ? now.ToString("MM-dd-yyyy hh:mm:ss"): "";
            string signStudent = signed ? $"{data.personal.surname} {data.personal.mothername} {data.personal.name}" : "";
            string signAceptedBy = signed ? "ANDRÉS SOTIL – DIRECTOR ACADÉMICO" : "";
            string signGuardian = "";
            if (data.guardian != null)
            {
                signGuardian = signed ? $"{data.guardian.surname} {data.guardian.name}" : "";
            }
            
            t.AddCell(new PdfPCell(new Paragraph($"Firma del Estudiante\n\n {signStudent}\n\n", fTnormal)));
            t.AddCell(new PdfPCell(new Paragraph($"Fecha\n\n{signDate}\n\n", fTnormal)));
            if(!isGreater18)
            {
                t.AddCell(new PdfPCell(new Paragraph($"Firma del padre o tutor\n\n{signGuardian}\n\n", fTnormal)));
                t.AddCell(new PdfPCell(new Paragraph($"Fecha\n\n{signDate}\n\n", fTnormal)));
            }
            t.AddCell(new PdfPCell(new Paragraph($"Aceptado por \n\n{signAceptedBy}\n\n", fTnormal)));
            t.AddCell(new PdfPCell(new Paragraph($"Fecha\n\n{signDate}\n\n", fTnormal)));

            return t;
        }

        private PdfPTable SigningAddendumTable(ApplicantInfo data, bool signed)
        {
            PdfPTable t = new PdfPTable(new float[] { 50f, 50f });
            DateTime now = DateTime.Now;
            bool isGreater18 = yearsDiff(data.personal.birthdate, now) >= 18;
            string signDate = signed ? now.ToString("MM-dd-yyyy hh:mm:ss") : "";
            string signStudent = signed ? $"{data.personal.surname} {data.personal.mothername} {data.personal.name}" : "";
            string signAceptedBy = signed ? "ANDRÉS SOTIL – DIRECTOR ACADÉMICO" : "";
            string signGuardian = "";
            if (data.guardian != null)
            {
                signGuardian = signed ? $"{data.guardian.surname} {data.guardian.name}" : "";
            }

            t.AddCell(new PdfPCell(new Paragraph($"Firma del Estudiante\n\n {signStudent}\n\n", fTnormal)));
            t.AddCell(new PdfPCell(new Paragraph($"Fecha\n\n{signDate}\n\n", fTnormal)));
            if (!isGreater18)
            {
                t.AddCell(new PdfPCell(new Paragraph($"Firma del padre o tutor\n\n{signGuardian}\n\n", fTnormal)));
                t.AddCell(new PdfPCell(new Paragraph($"Fecha\n\n{signDate}\n\n", fTnormal)));
            }
            t.AddCell(new PdfPCell(new Paragraph($"Aceptado por \n\n{signAceptedBy}\n\n", fTnormal)));
            t.AddCell(new PdfPCell(new Paragraph($"Fecha\n\n{signDate}\n\n", fTnormal)));

            return t;
        }

        private int yearsDiff(DateTime start, DateTime end)
        {
            return (end.Year - start.Year - 1) +
                (((end.Month > start.Month) ||
                ((end.Month == start.Month) && (end.Day >= start.Day))) ? 1 : 0);
        }
    }
}

