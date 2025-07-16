using System;
using System.Collections.Generic;

[Serializable]
public class Choice
{
    public string text;
    public string nextScene;
}

[Serializable]
public class DialogueLine
{
    public string character;
    public string characterName;
    public string characterSprite;
    public string text;
    public string background;
    public List<Choice> choices;
}

[Serializable]
public class SceneData
{
    public string sceneName;
    public List<DialogueLine> dialogues;
}
