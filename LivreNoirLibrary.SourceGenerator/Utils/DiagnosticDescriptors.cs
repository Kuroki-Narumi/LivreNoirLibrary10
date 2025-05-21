using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

public static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor GenericType = new(
        id: "LNLG-P01",
        title: "Generic Type",
        messageFormat: "The class '{0}' has generic parameters but it is not allowed.",
        category: "PropertyGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}