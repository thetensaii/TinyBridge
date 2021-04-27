using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

#region GameManager Events
public class GameMenuEvent : SDD.Events.Event
{
}
public class GameLevelsMenuEvent : SDD.Events.Event
{
}
public class GamePlayEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class GameVictoryEvent : SDD.Events.Event
{
}

public class GameStatisticsChangedEvent : SDD.Events.Event
{
	public int eNodesNumber { get; set; }
}

public class AskToGoToNextLevelEvent : SDD.Events.Event
{
}
#endregion

#region MenuManager Events
public class EscapeButtonClickedEvent : SDD.Events.Event
{
}
public class PlayButtonClickedEvent : SDD.Events.Event
{
}
public class ResumeButtonClickedEvent : SDD.Events.Event
{
}
public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}

public class QuitButtonClickedEvent : SDD.Events.Event
{ }

public class LevelsButtonClickedEvent : SDD.Events.Event
{
}
public class BackButtonClickedEvent : SDD.Events.Event
{
}

public class Level1ButtonClickedEvent : SDD.Events.Event
{
}

public class Level2ButtonClickedEvent : SDD.Events.Event
{
}
public class Level3ButtonClickedEvent : SDD.Events.Event
{
}

public class RestartButtonClickedEvent : SDD.Events.Event
{
}

public class NextLevelButtonClickedEvent : SDD.Events.Event
{

}
#endregion

public class NodeHasBeenCreatedEvent : SDD.Events.Event
{

}

public class AllNodeHaveBeenCreatedEvent : SDD.Events.Event
{

}

public class GoalHasBeenReachedEvent : SDD.Events.Event
{

}

public class StructureFinishedEvent : SDD.Events.Event
{

}

#region Level Event
public class GoToLevel1Event : SDD.Events.Event
{

}
public class GoToLevel2Event : SDD.Events.Event
{

}
public class GoToLevel3Event : SDD.Events.Event
{

}
public class RestartLevelEvent : SDD.Events.Event
{

}

public class GoToNextLevelEvent : SDD.Events.Event
{
}
public class SceneHasBeenLoadedEvent : SDD.Events.Event
{
    public int eLevelLoaded { get; set; }
}
#endregion

#region HudManager Event
public class StructureFinishedButtonClickedEvent : SDD.Events.Event
{

}
#endregion

#region DeathZone Event
public class PlayerHasHitDeathZoneEvent : SDD.Events.Event
{

}
#endregion