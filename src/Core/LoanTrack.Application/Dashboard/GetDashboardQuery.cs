using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Dashboard;

public record GetDashboardQuery() : IQuery<DashboardResponse>;
