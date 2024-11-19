using System.Reflection;
using ResultBoxes;
using Sekiban.Core.Aggregate;
using Sekiban.Core.Command;
using Sekiban.Core.Dependency;
using Sekiban.Core.Events;

namespace SekibanPrj.Domain;

public class ProjectDependency : DomainDependencyDefinitionBase
{
    public override Assembly GetExecutingAssembly() => Assembly.GetExecutingAssembly();

    public override void Define()
    {
    }
}

public record BaseballTeam(string Name, string City, string League) : IAggregatePayload<BaseballTeam>
{
    public static BaseballTeam CreateInitialPayload(BaseballTeam? _) => new(string.Empty, string.Empty, string.Empty);
}

public record BaseballTeamCreated(string Name, string City, string League) : IEventPayload<BaseballTeam, BaseballTeamCreated>
{
    public static BaseballTeam OnEvent(BaseballTeam aggregatePayload, Event<BaseballTeamCreated> ev)
    => aggregatePayload with { Name = ev.Payload.Name, City = ev.Payload.City, League = ev.Payload.League };
}

public record CreateBaseballTeam(string Name, string City, string League) : ICommandWithHandler<BaseballTeam, CreateBaseballTeam>
{
    public static Guid SpecifyAggregateId(CreateBaseballTeam command) => Guid.CreateVersion7();

    public static ResultBox<EventOrNone<BaseballTeam>> HandleCommand(CreateBaseballTeam command, ICommandContext<BaseballTeam> context)
    => EventOrNone.Event(new BaseballTeamCreated(command.Name, command.City, command.League));
}

public record BaseballTeamNameChanged(string Name) : IEventPayload<BaseballTeam, BaseballTeamNameChanged>
{
    public static BaseballTeam OnEvent(BaseballTeam aggregatePayload, Event<BaseballTeamNameChanged> ev)
    => aggregatePayload with { Name = ev.Payload.Name };
}

public record ChangeBaseballTeamName(Guid BaseballTeamId, string Name) : ICommandWithHandler<BaseballTeam, ChangeBaseballTeamName>
{
    public static ResultBox<EventOrNone<BaseballTeam>> HandleCommand(ChangeBaseballTeamName command, ICommandContext<BaseballTeam> context)
    => EventOrNone.Event(new BaseballTeamNameChanged(command.Name));

    public static Guid SpecifyAggregateId(ChangeBaseballTeamName command) => command.BaseballTeamId;
}
