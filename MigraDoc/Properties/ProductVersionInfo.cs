#region MigraDoc - Creating Documents on the Fly
//
// Copyright (c) 2001-2017 empira Software GmbH, Cologne Area (Germany)
//
// http://www.pdfsharp.com
// http://www.migradoc.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System;

namespace MigraDoc
{
    /// <summary>
    /// Base namespace of MigraDoc. Classes are implemented in nested namespaces like e. g. MigraDoc.DocumentObjectModel.
    /// </summary>
    /// <seealso cref="MigraDoc.DocumentObjectModel"></seealso>
    [System.Runtime.CompilerServices.CompilerGenerated]
    internal class NamespaceDoc { }

    /// <summary>
    /// Version info base for all MigraDoc related assemblies.
    /// </summary>
    public static class ProductVersionInfo
    {
        /// <summary>
        /// The title of the product.
        /// </summary>
        public const string Title = "empira MigraDoc";

        /// <summary>
        /// A characteristic description of the product.
        /// </summary>
        public const string Description = "Creating Documents on the Fly";

        /// <summary>
        /// The PDF creator information string.
        /// TODO: Called Producer in PdfSharp???
        /// </summary>
        public const string Creator = Title + " " + VersionMajor + "." + VersionMinor + "." + VersionBuild + " (" + Url + ")";

        /// <summary>
        /// The full version number.
        /// </summary>
        public const string Version = VersionMajor + "." + VersionMinor + "." + VersionBuild + "." + VersionPatch;

        /// <summary>
        /// The home page of this product.
        /// </summary>
        public const string Url = "www.migradoc.com";

        /// <summary>
        /// Unused.
        /// </summary>
        public const string Configuration = "";

        /// <summary>
        /// The company that created/owned the product.
        /// </summary>
        public const string Company = "empira Software GmbH, Cologne Area (Germany)";

        /// <summary>
        /// The name the product.
        /// </summary>
        public const string Product = "empira MigraDoc";

        /// <summary>
        /// The copyright information.
        /// </summary>
        public const string Copyright = "Copyright � 2001-2017 empira Software GmbH."; // Also used as NuGet Copyright.

        /// <summary>
        /// The trademark the product.
        /// </summary>
        public const string Trademark = "empira MigraDoc";

        /// <summary>
        /// Unused.
        /// </summary>
        public const string Culture = "";

        // Build = days since 2001-07-04 - change values ONLY here

        /// <summary>
        /// The major version number of the product.
        /// </summary>
        public const string VersionMajor = "1"; // Also used for NuGet Version.

        /// <summary>
        /// The minor version number of the product.
        /// </summary>
        public const string VersionMinor = "50"; // Also used for NuGet Version.

        /// <summary>
        /// The build number of the product.
        /// </summary>
        public const string VersionBuild = "4740"; // V16G //"4212"; // Also used for NuGet Version.

        /// <summary>
        /// The patch number of the product.
        /// </summary>
        public const string VersionPatch = "0"; // Also used for NuGet Version.

        /// <summary>
        /// The Version Prerelease String for NuGet.
        /// </summary>
        public const string VersionPrerelease = "beta5"; // "" for stable Release, e.g. "beta" or "rc.1.2" for Prerelease. // Also used for NuGet Version.

#if DEBUG
// ReSharper disable RedundantNameQualifier
        //public static int BuildNumber = (System.DateTime.Now - new System.DateTime(2001, 7, 4)).Days;
        public static int BuildNumber = (System.DateTime.Now - new System.DateTime(2005, 1, 1)).Days; // Same BuildNumber like PDFsharp
// ReSharper restore RedundantNameQualifier
#endif

        /// <summary>
        /// E.g. "2005-01-01", for use in NuGet Script
        /// </summary>
        public const string VersionReferenceDate = "2005-01-01";

        /// <summary>
        /// Use _ instead of blanks and special characters. Can be complemented with a suffix in the NuGet Script.
        /// Nuspec Doc: The unique identifier for the package. This is the package name that is shown when packages are listed using the Package Manager Console.
        /// These are also used when installing a package using the Install-Package command within the Package Manager Console.
        /// Package IDs may not contain any spaces or characters that are invalid in an URL. In general, they follow the same rules as .NET namespaces do.
        /// So Foo.Bar is a valid ID, Foo! and Foo Bar are not. 
        /// </summary>
        public const string NuGetID = "PDFsharp-MigraDoc";

        /// <summary>
        /// Nuspec Doc: The human-friendly title of the package displayed in the Manage NuGet Packages dialog. If none is specified, the ID is used instead. 
        /// </summary>
        public const string NuGetTitle = "PDFsharp + MigraDoc";

        /// <summary>
        /// Nuspec Doc: A comma-separated list of authors of the package code.
        /// </summary>
        public const string NuGetAuthors = "empira Software GmbH";

        /// <summary>
        /// Nuspec Doc: A comma-separated list of the package creators. This is often the same list as in authors. This is ignored when uploading the package to the NuGet.org Gallery. 
        /// </summary>
        public const string NuGetOwners = "empira Software GmbH";

        /// <summary>
        /// Nuspec Doc: A long description of the package. This shows up in the right pane of the Add Package Dialog as well as in the Package Manager Console when listing packages using the Get-Package command. 
        /// </summary>
        public const string NuGetDescription = "MigraDoc Foundation - the Open Source .NET library that easily creates documents based on an object model with paragraphs, tables, styles, etc. and renders them into PDF or RTF.";

        /// <summary>
        /// Nuspec Doc: A description of the changes made in each release of the package. This field only shows up when the _Updates_ tab is selected and the package is an update to a previously installed package.
        /// It is displayed where the Description would normally be displayed. 
        /// </summary>                  
        public const string NuGetReleaseNotes = "";

        /// <summary>
        /// Nuspec Doc: A short description of the package. If specified, this shows up in the middle pane of the Add Package Dialog. If not specified, a truncated version of the description is used instead.
        /// </summary>                  
        public const string NuGetSummary = "Creating Documents on the Fly.";

        /// <summary>
        /// Nuspec Doc: The locale ID for the package, such as en-us.
        /// </summary>                  
        public const string NuGetLanguage = "";

        /// <summary>
        /// Nuspec Doc: A URL for the home page of the package.
        /// </summary>
        public const string NuGetProjectUrl = "http://www.pdfsharp.net/";

        /// <summary>
        /// Nuspec Doc: A URL for the image to use as the icon for the package in the Manage NuGet Packages dialog box. This should be a 32x32-pixel .png file that has a transparent background.
        /// </summary>
        public const string NuGetIconUrl = "http://www.pdfsharp.net/resources/MigraDoc-Logo-32x32.png";

        /// <summary>
        /// Nuspec Doc: A link to the license that the package is under.
        /// </summary>                  
        public const string NuGetLicenseUrl = "http://www.pdfsharp.net/MigraDoc_License.ashx";

        /// <summary>
        /// Nuspec Doc: A Boolean value that specifies whether the client needs to ensure that the package license (described by licenseUrl) is accepted before the package is installed.
        /// </summary>                  
        public const bool NuGetRequireLicenseAcceptance = false;

        /// <summary>
        /// Nuspec Doc: A space-delimited list of tags and keywords that describe the package. This information is used to help make sure users can find the package using
        /// searches in the Add Package Reference dialog box or filtering in the Package Manager Console window.
        /// </summary>                  
        public const string NuGetTags = "MigraDoc PdfSharp Pdf Document Generation";
    }
}
