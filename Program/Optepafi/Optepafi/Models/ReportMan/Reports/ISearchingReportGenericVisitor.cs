namespace Optepafi.Models.ReportMan.Reports;


/// <summary>
/// One of generic visitor interfaces for <see cref="ISearchingReport"/> implementations.
///
/// It provides access to modified visitor pattern on searching reports, where only one generic method is required to be implemented.  
/// It serves mainly for acquiring generic parameter, that represents real type of visited object.  
/// It has one more overload for convenience of use.  
/// </summary>
/// <typeparam name="TOut">Specifies type of returned value of <c>GenericVisit</c>.</typeparam>
/// <typeparam name="TOtherParams">Specifies types of rest of the parameters carried through visitor pattern.</typeparam>
public interface ISearchingReportGenericVisitor<TOut, TOtherParams>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// </summary>
    /// <param name="searchingReport">Searching report which accepted the visit.</param>
    /// <param name="otherParams">Other parameters carried through visitor pattern.</param>
    /// <typeparam name="TSearchingReport">Type of accepting searching report. Main result of this modified visitor pattern.</typeparam>
    /// <returns>Value of type <c>TOut</c>.</returns>
    TOut GenericVisit<TSearchingReport>(TSearchingReport searchingReport, TOtherParams otherParams) where TSearchingReport : ISearchingReport;
}

/// <summary>
/// One of generic visitor interfaces for <see cref="ISearchingReport"/> implementations.
/// 
/// It provides access to modified visitor pattern on searching reports.  
/// For more information see <see cref="ISearchingReportGenericVisitor{TOut,TOtherParams}"/>.  
/// </summary>
public interface ISearchingReportGenericVisitor<TOut>
{
    /// <summary>
    /// Visiting method to be implemented.
    /// 
    /// For more information of this method see <see cref="ISearchingReportGenericVisitor{TOut,TOtherParams}"/>.
    /// </summary>
    TOut GenericVisit<TSearchingReport>(TSearchingReport searchingReport) where TSearchingReport : ISearchingReport;
}