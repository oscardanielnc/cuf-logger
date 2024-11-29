using System;
using cuf_admision_domain.Entities.Application;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using System.Reflection.Metadata;
using Document = iTextSharp.text.Document;
using System.Reflection.Emit;
using cuf_admision_domain.Entities.Payment;
using cuf_admision_domain;
using static iTextSharp.text.pdf.AcroFields;

namespace cuf_admision_pdf.Documets
{
    public class InvoiceDocument
    {
        public string documentName { get; set; }
        //private readonly ApplicationPayment _data;
        //private readonly PaymentDetails _payment;
        private readonly InvoiceDetails _payment;

        Font fTitle;//new Font(Font.NORMAL, 18);
        Font fSs;
        Font fTBold;
        Font fTnormal;

        public InvoiceDocument
            (string filepath,
            InvoiceDetails payment)
        {
            documentName = filepath;
            _payment = payment;

            System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);

            string startupPath = Environment.CurrentDirectory;
            BaseFont customfont = BaseFont.CreateFont(
                startupPath + @"/Roboto/Roboto-Regular.ttf",
                BaseFont.CP1252, BaseFont.EMBEDDED);
            BaseFont customfontBold = BaseFont.CreateFont(
                startupPath + @"/Roboto/Roboto-Medium.ttf",
                BaseFont.CP1252, BaseFont.EMBEDDED);

            //Font fTitle = FontFactory.GetFont("Arial", 18, BaseColor.BLACK);//new Font(Font.NORMAL, 18);
            fTitle = new Font(customfontBold, 18, Font.BOLD);
            fSs = new Font(customfontBold, 12, Font.BOLD);
            fTBold = new Font(customfontBold, 8, Font.BOLD);
            fTnormal = new Font(customfont, 8, Font.NORMAL);
        }

        public void Generate()
        {
            FileStream fs = new FileStream(documentName, FileMode.Create, FileAccess.Write, FileShare.None);
            Rectangle rec = new Rectangle(PageSize.A4);
            Document doc = new Document(rec);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();

            PdfPTable layoutTable = new PdfPTable(new float[] { 100f });
            layoutTable.DefaultCell.Border = Rectangle.NO_BORDER;


            //Paragraph doc = new Paragraph();

            PdfPTable headerTable = new PdfPTable(new float[] { 30f, 20f, 30f });
            string startupPath = Environment.CurrentDirectory;
            string imageURL = startupPath + "/Logos/logo-cuf.png";
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imageURL);
            logo.ScaleToFit(140f, 100f);
            //logo.SpacingBefore = 10f;
            //logo.SpacingAfter = 1f;
            logo.Alignment = Element.ALIGN_LEFT;

            //headerTable.SpacingBefore = 0;
            // headerTable.SpacingAfter = 0;
            //headerTable.DefaultCell.BorderWidth = 0;
            headerTable.DefaultCell.Border = Rectangle.NO_BORDER;
            //headerTable.DefaultCell.BorderColor = BaseColor.WHITE;

            headerTable.AddCell(new PdfPCell(logo) { Border = Rectangle.NO_BORDER });
            headerTable.AddCell(new PdfPCell(new Paragraph("", fTnormal)) { Border = Rectangle.NO_BORDER });
            headerTable.AddCell(new PdfPCell(new Paragraph(
                "Servicios@continentaluniversity.us\n" +
                "5201 Blue Lagoon Drive / 8th Floor & 9th " +
                "Floor, Miami 33126,Florida\n" +
                "Telf/Phone: (786) 220-2888", fTnormal))
            { Border = Rectangle.NO_BORDER });

            layoutTable.AddCell(headerTable);

            layoutTable.AddCell(new Paragraph("\n\n"));

            layoutTable.AddCell(new Paragraph("Recibo de Pago/ Payment Receipt", fTBold));
            layoutTable.AddCell(new Paragraph("\n\n"));

            layoutTable.AddCell(AbstractTable());
            layoutTable.AddCell(new Paragraph("\n\n"));

            layoutTable.AddCell(new Paragraph("Detalle/Detail:", fTBold));

            layoutTable.AddCell(DetailTable());

            //layoutTable.AddCell(doc);

            doc.Add(layoutTable);
            
            var content = writer.DirectContent;
            var pageBorderRect = new Rectangle(doc.PageSize);

            pageBorderRect.Left += doc.LeftMargin;
            pageBorderRect.Right -= doc.RightMargin;
            pageBorderRect.Top -= doc.TopMargin;
            pageBorderRect.Bottom += doc.BottomMargin;

            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
            content.Stroke();
            

            doc.Close();
        }

        private PdfPTable AbstractTable()
        {
            PdfPTable t = new PdfPTable(new float[] { 40f, 60f });
            t.DefaultCell.Border = Rectangle.NO_BORDER;

            t.AddCell(new PdfPCell(new Paragraph("Recibo Nro.:", fTBold)) { Border = Rectangle.NO_BORDER });
            t.AddCell(new PdfPCell(new Paragraph($"{CufUtils.ComposeInvoiceCode(_payment.invoice.id)}", fTnormal)) { Border = Rectangle.NO_BORDER });

            t.AddCell(new PdfPCell(new Paragraph("Forma Pago/Payment Form:", fTBold)) { Border = Rectangle.NO_BORDER });
            t.AddCell(new PdfPCell(new Paragraph(_payment.invoice.paymentMethod, fTnormal)) { Border = Rectangle.NO_BORDER });

            t.AddCell(new PdfPCell(new Paragraph("Referenc.:", fTBold)) { Border = Rectangle.NO_BORDER });
            t.AddCell(new PdfPCell(new Paragraph($"{_payment.invoice.reference}", fTnormal)) { Border = Rectangle.NO_BORDER });

            t.AddCell(new PdfPCell(new Paragraph("Estudiante/Student:", fTBold)) { Border = Rectangle.NO_BORDER });
            t.AddCell(new PdfPCell(new Paragraph($"{_payment.invoice.admisionName}", fTnormal)) { Border = Rectangle.NO_BORDER });

            //t.AddCell(new PdfPCell(new Paragraph("ID Estudiante/Student ID", fTBold)) { Border = Rectangle.NO_BORDER });
            //t.AddCell(new PdfPCell(new Paragraph(_payment.invoice.admisionCode, fTnormal)) { Border = Rectangle.NO_BORDER });

            t.AddCell(new PdfPCell(new Paragraph("Fecha Tran./Tran.Date:", fTBold)) { Border = Rectangle.NO_BORDER });
            t.AddCell(new PdfPCell(new Paragraph($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", fTnormal)) { Border = Rectangle.NO_BORDER });

            t.AddCell(new PdfPCell(new Paragraph("Periodo/Period", fTBold)) { Border = Rectangle.NO_BORDER });
            t.AddCell(new PdfPCell(new Paragraph($"{_payment.invoice.termName}", fTnormal)) { Border = Rectangle.NO_BORDER });


            return t;
        }

        private PdfPTable DetailTable()
        {
            decimal subTotal = _payment.details
                .Select(f => f.amount).Sum();
            decimal discounts = 0;
            decimal total = subTotal - discounts;

            PdfPTable t = new PdfPTable(new float[] { 60f, 40f });

            t.AddCell(new PdfPCell(new Paragraph("Descripción/Description", fTBold)) { BorderWidthRight = 0 });
            t.AddCell(new PdfPCell(new Paragraph("Monto/Amount", fTBold)) { BorderWidthLeft = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

            List l2 = new List(20f);
            l2.SetListSymbol("\u2022");

            foreach(var item in _payment.details)
            {
                l2.Add(new ListItem($"{item.conceptName}: US$ {string.Format("{0:#.00}", item.amount)}", fTnormal));

            }
            

            List l1 = new List(20f);
            //l1.SetListSymbol("\u2022");
            l1.Add(new ListItem("Cuotas Afectadas/ Affected Fees:", fTnormal));
            l1.Add(l2);


            //Paragraph desc = new Paragraph();

            //desc.Add(new Paragraph("Course fee payment", fTnormal));
            //desc.Add(l1);
            PdfPCell cell = new PdfPCell() { BorderWidthRight = 0};
            cell.AddElement(new Paragraph("Course fee payment", fTnormal));
            cell.AddElement(l1);


            t.AddCell(cell);
            t.AddCell(new PdfPCell(new Paragraph($"US$ {string.Format("{0:#.00}", subTotal)}", fTBold)) { BorderWidthLeft = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

            t.AddCell(new PdfPCell(new Paragraph("")) { BorderWidthRight = 0 });

            PdfPTable payTable = new PdfPTable(new float[] { 70f, 30f });
            payTable.DefaultCell.Border = Rectangle.NO_BORDER;

            payTable.AddCell(new Paragraph("Sub Total:", fTnormal));
            payTable.AddCell(new Paragraph($"US$ {string.Format("{0:#.00}", subTotal)}:", fTnormal));

            // payTable.AddCell(new Paragraph("(-) Descuentos/Discounts:", fTnormal));
            // payTable.AddCell(new Paragraph($"US$ {string.Format("{0:#.00}", discounts)}:", fTnormal));

            payTable.AddCell(new Paragraph("(+) Recargos/Charges:", fTnormal));
            payTable.AddCell(new Paragraph($"US$ 0.00", fTnormal));

            payTable.AddCell(new Paragraph("(=) Total:", fTnormal));
            payTable.AddCell(new Paragraph($"US$ {string.Format("{0:#.00}", total)}", fTnormal));


            t.AddCell(new PdfPCell(payTable)
            { BorderWidthLeft = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

            return t;
        }

    }
    /*
    public class CustomDocument: Document
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            var content = writer.DirectContent;
            var pageBorderRect = new Rectangle(document.PageSize);

            pageBorderRect.Left += document.LeftMargin;
            pageBorderRect.Right -= document.RightMargin;
            pageBorderRect.Top -= document.TopMargin;
            pageBorderRect.Bottom += document.BottomMargin;

            content.SetColorStroke(BaseColor.RED);
            content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
            content.Stroke();
        }
    }*/
}

