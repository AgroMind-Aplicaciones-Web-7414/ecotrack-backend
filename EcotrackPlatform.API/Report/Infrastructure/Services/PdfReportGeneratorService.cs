namespace EcotrackPlatform.API.Report.Infrastructure.Services;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text.Json;
using System.Linq;

/// <summary>
/// Servicio para generar reportes en formato PDF
/// </summary>
public class PdfReportGeneratorService
{
    public byte[] GenerateTasksReportPdf(object reportDataObj)
    {
        // Configurar la licencia de QuestPDF (Community license)
        QuestPDF.Settings.License = LicenseType.Community;

        // Deserializar el objeto a un JsonElement para acceder a las propiedades
        var jsonString = JsonSerializer.Serialize(reportDataObj);
        using var document = JsonDocument.Parse(jsonString);
        var root = document.RootElement;

        // Extraer los datos del reporte (usando PascalCase como C# los genera)
        var summary = root.GetProperty("Summary");
        var totalTasks = summary.GetProperty("TotalTasks").GetInt32();
        var completedTasks = summary.GetProperty("CompletedTasks").GetInt32();
        var inProgressTasks = summary.GetProperty("InProgressTasks").GetInt32();
        var pendingTasks = summary.GetProperty("PendingTasks").GetInt32();
        var completionRate = summary.GetProperty("CompletionRate").GetDouble();

        var tasksArray = root.GetProperty("Tasks").EnumerateArray().ToList();

        var pdfDocument = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                // Header
                page.Header()
                    .Background(Colors.Blue.Lighten3)
                    .Padding(15)
                    .Column(column =>
                    {
                        column.Item().Text("AGROMIND ECOTRACK")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);
                        
                        column.Item().Text("Reporte de Estado de Tareas")
                            .FontSize(14)
                            .FontColor(Colors.Grey.Darken1);
                        
                        column.Item().PaddingTop(5).Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken1);
                    });

                // Content
                page.Content()
                    .PaddingVertical(10)
                    .Column(column =>
                    {
                        // Summary Section
                        column.Item().Text("RESUMEN EJECUTIVO")
                            .FontSize(14)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);
                        
                        column.Item().PaddingTop(8).PaddingBottom(10).Row(row =>
                        {
                            // Total Tasks Card
                            row.RelativeItem().Padding(3).Border(1).BorderColor(Colors.Grey.Lighten2)
                                .Background(Colors.Blue.Lighten4).Padding(8).Column(col =>
                                {
                                    col.Item().Text("Total")
                                        .FontSize(9)
                                        .FontColor(Colors.Grey.Darken1);
                                    col.Item().Text(totalTasks.ToString())
                                        .FontSize(20)
                                        .Bold()
                                        .FontColor(Colors.Blue.Darken2);
                                });

                            // Completed Tasks Card
                            row.RelativeItem().Padding(3).Border(1).BorderColor(Colors.Grey.Lighten2)
                                .Background(Colors.Green.Lighten4).Padding(8).Column(col =>
                                {
                                    col.Item().Text("Completadas")
                                        .FontSize(9)
                                        .FontColor(Colors.Grey.Darken1);
                                    col.Item().Text(completedTasks.ToString())
                                        .FontSize(20)
                                        .Bold()
                                        .FontColor(Colors.Green.Darken2);
                                });

                            // In Progress Tasks Card
                            row.RelativeItem().Padding(3).Border(1).BorderColor(Colors.Grey.Lighten2)
                                .Background(Colors.Orange.Lighten4).Padding(8).Column(col =>
                                {
                                    col.Item().Text("En Progreso")
                                        .FontSize(9)
                                        .FontColor(Colors.Grey.Darken1);
                                    col.Item().Text(inProgressTasks.ToString())
                                        .FontSize(20)
                                        .Bold()
                                        .FontColor(Colors.Orange.Darken2);
                                });

                            // Pending Tasks Card
                            row.RelativeItem().Padding(3).Border(1).BorderColor(Colors.Grey.Lighten2)
                                .Background(Colors.Grey.Lighten3).Padding(8).Column(col =>
                                {
                                    col.Item().Text("Pendientes")
                                        .FontSize(9)
                                        .FontColor(Colors.Grey.Darken1);
                                    col.Item().Text(pendingTasks.ToString())
                                        .FontSize(20)
                                        .Bold()
                                        .FontColor(Colors.Grey.Darken2);
                                });
                            
                            // Completion Rate Card
                            row.RelativeItem().Padding(3).Border(1).BorderColor(Colors.Grey.Lighten2)
                                .Background(Colors.Teal.Lighten4).Padding(8).Column(col =>
                                {
                                    col.Item().Text("Tasa")
                                        .FontSize(9)
                                        .FontColor(Colors.Grey.Darken1);
                                    col.Item().Text($"{completionRate}%")
                                        .FontSize(20)
                                        .Bold()
                                        .FontColor(Colors.Teal.Darken2);
                                });
                        });

                        // Tasks Table
                        column.Item().PaddingTop(10).Text("DETALLE DE TAREAS")
                            .FontSize(14)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        column.Item().PaddingTop(8).Table(table =>
                        {
                            // Define columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);  // ID
                                columns.RelativeColumn(3);   // Title
                                columns.ConstantColumn(50);  // Responsible
                                columns.RelativeColumn(1.2f); // Status
                                columns.RelativeColumn(1);   // Created At
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                                    .Text("ID").FontColor(Colors.White).Bold().FontSize(8);
                                header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                                    .Text("Título").FontColor(Colors.White).Bold().FontSize(8);
                                header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                                    .Text("Resp.").FontColor(Colors.White).Bold().FontSize(8);
                                header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                                    .Text("Estado").FontColor(Colors.White).Bold().FontSize(8);
                                header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                                    .Text("Creada").FontColor(Colors.White).Bold().FontSize(8);
                            });

                            // Rows
                            int rowIndex = 0;
                            foreach (var taskElement in tasksArray)
                            {
                                var backgroundColor = rowIndex % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;
                                rowIndex++;

                                // Usar métodos seguros para obtener valores
                                var taskId = GetIntValue(taskElement, "Id");
                                var taskTitle = taskElement.GetProperty("Title").GetString() ?? "N/A";
                                var taskResponsibleId = GetIntValue(taskElement, "ResponsibleId");
                                var taskStatus = taskElement.GetProperty("Status").GetString() ?? "Pending";
                                var taskCreatedAt = GetDateTimeValue(taskElement, "CreatedAt");

                                table.Cell().Background(backgroundColor).Padding(4)
                                    .Text(taskId.ToString()).FontSize(8);
                                table.Cell().Background(backgroundColor).Padding(4)
                                    .Text(taskTitle).FontSize(8);
                                table.Cell().Background(backgroundColor).Padding(4)
                                    .Text(taskResponsibleId.ToString()).FontSize(8);
                                
                                // Status with color
                                var statusColor = taskStatus switch
                                {
                                    "Completed" => Colors.Green.Darken1,
                                    "InProgress" => Colors.Orange.Darken1,
                                    _ => Colors.Grey.Darken1
                                };
                                
                                table.Cell().Background(backgroundColor).Padding(4)
                                    .Text(GetStatusSpanish(taskStatus))
                                    .FontSize(8)
                                    .FontColor(statusColor)
                                    .Bold();
                                
                                table.Cell().Background(backgroundColor).Padding(4)
                                    .Text(taskCreatedAt.ToString("dd/MM/yy"))
                                    .FontSize(8);
                            }
                        });
                    });

                // Footer
                page.Footer()
                    .AlignCenter()
                    .DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Grey.Darken1))
                    .Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                        x.Span(" | © 2025 Agromind Ecotrack Platform");
                    });
            });
        });

        return pdfDocument.GeneratePdf();
    }

    private string GetStatusSpanish(string status)
    {
        return status switch
        {
            "Completed" => "Completada",
            "InProgress" => "En Progreso",
            "Pending" => "Pendiente",
            _ => status
        };
    }

    /// <summary>
    /// Obtiene un valor entero de manera segura desde un JsonElement
    /// </summary>
    private int GetIntValue(JsonElement element, string propertyName)
    {
        var property = element.GetProperty(propertyName);
        
        // Intentar obtener como número directamente
        if (property.ValueKind == JsonValueKind.Number)
        {
            return property.GetInt32();
        }
        
        // Si es string, intentar parsearlo
        if (property.ValueKind == JsonValueKind.String)
        {
            var stringValue = property.GetString();
            if (int.TryParse(stringValue, out var result))
            {
                return result;
            }
        }
        
        return 0;
    }

    /// <summary>
    /// Obtiene un valor DateTime de manera segura desde un JsonElement
    /// </summary>
    private DateTime GetDateTimeValue(JsonElement element, string propertyName)
    {
        var property = element.GetProperty(propertyName);
        
        // Intentar obtener como DateTime directamente
        if (property.TryGetDateTime(out var dateTime))
        {
            return dateTime;
        }
        
        // Si es string, intentar parsearlo
        if (property.ValueKind == JsonValueKind.String)
        {
            var stringValue = property.GetString();
            if (DateTime.TryParse(stringValue, out var result))
            {
                return result;
            }
        }
        
        return DateTime.MinValue;
    }
}

