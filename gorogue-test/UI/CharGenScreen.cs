using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;
using Microsoft.Xna.Framework;
using System;
using GoRogueTest.Entity;
using System.Collections.Generic;
using System.Linq;
using GoRogue.Random;

namespace GoRogueTest.UI
{
  public class CharGenScreen: Screen
  {
    private const int RACEW = 48;
    private const int RACEH = 21;
    private const int RACEX = 1;
    private const int RACEY = 1;
    private const int TRAITW = 48;
    private const int TRAITH = 21;
    private const int TRAITX = 51;
    private const int TRAITY = 1;
    private const int STATW = 98;
    private const int STATH = 15;
    private const int STATX = 1;
    private const int STATY = 24;
    private const int STATCOL = 0;
    private const int TOTALCOL = 9;
    private const int RACECOL = 18;
    private const int TRAITCOL = 27;
    private const int SPENTCOL = 36;
    private const int HEADROW = 4;
    private const int STRROW = 5;
    private const int STAMROW = 6;
    private const int SPDROW = 7;
    private const int SKLROW = 8;
    private const int SAGROW = 9;
    private const int SMTROW = 10;
    private const int TOOLW = 27;
    private const int TOOLH = 9;
    private const int TOOLX = 21;
    private const int TOOLY = 12;
    private const int TRAITTOOLW = 27;
    private const int TRAITTOOLH = 8;
    private const int TRAITTOOLX = 21;
    private const int TRAITTOOLY = 13;
    

    private ControlsConsole races = new ControlsConsole(RACEW, RACEH);
    private ControlsConsole traits = new ControlsConsole(TRAITW, TRAITH);
    private ControlsConsole stats = new ControlsConsole(STATW, STATH);
    private string selectedRace = "";
    private readonly Dictionary <string, string> raceDisplay = new Dictionary<string, string>();
    private readonly Dictionary <string, string> traitDisplay = new Dictionary<string, string>();
    private int str = 0;
    private int stam = 0;
    private int spd = 0;
    private int skl = 0;
    private int sag = 0;
    private int smt = 0;

    private const int CP = 20;
    private int strTotal => RaceTemplates.templates[selectedRace].BaseStats.Str + str + getTraitTotals()[0];
    private int stamTotal => RaceTemplates.templates[selectedRace].BaseStats.Stam + stam + getTraitTotals()[1];
    private int spdTotal => RaceTemplates.templates[selectedRace].BaseStats.Spd + spd + getTraitTotals()[2];
    private int sklTotal => RaceTemplates.templates[selectedRace].BaseStats.Skl + skl + getTraitTotals()[3];
    private int sagTotal => RaceTemplates.templates[selectedRace].BaseStats.Sag + sag + getTraitTotals()[4];
    private int smtTotal => RaceTemplates.templates[selectedRace].BaseStats.Smt + smt + getTraitTotals()[5];
    private ControlBase toolOn;
    private int spentCP => str + stam + spd + skl + sag + smt;
    private bool overspent => spentCP > CP;
    private bool underspent => spentCP < CP;
  
    public CharGenScreen() : base("chargen") {}

    protected override void Init() {
      InitRaces();
      InitTraits();
      InitStats();      
    }
    public override void HandleKeys()
    {
        //do nothing
    }

    private void InitRaces()
    {
      int counter = 0;
      var tooltip = new Window(TOOLW, TOOLH);
      bool first = true;
      UIUtils.border(races, "Races");
      UIUtils.border(tooltip);
      foreach (var race in RaceTemplates.templates)
      {
        raceDisplay[race.Value.Desc] = race.Key;
        var chk = new RadioButton(race.Value.Desc.Length + 4, 1);
        chk.Text = race.Value.Desc;
        chk.Position = new Point(1, counter);
        //chk.Theme = (RadioButtonTheme)ThemeLibrary.BWRadioTheme.Clone();
        chk.IsSelectedChanged += (snd, arg) =>
        {
          var clicked = snd as RadioButton;
          if (clicked != null)
          {
            if (clicked.IsSelected) 
            {
              stats.Clear(new Rectangle(RACECOL, STRROW, 8, 6));  
              selectedRace = raceDisplay[clicked.Text];
              var raceInfo = RaceTemplates.templates[selectedRace];
              var raceInfoBase = raceInfo.BaseStats;
              var raceStats = new int[]{
                raceInfoBase.Str,
                raceInfoBase.Stam,
                raceInfoBase.Spd,
                raceInfoBase.Skl,
                raceInfoBase.Sag,
                raceInfoBase.Smt
              };
              for (int i=0; i<raceStats.Length; i++) 
              {
                stats.Print(RACECOL, i+STRROW, $"{raceStats[i], 8}");
              }
            }
            UpdateAllStats();
          }
        };
        chk.MouseEnter += (snd, args) =>
        {
          var moused = snd as RadioButton;
          if (moused != null)
          {
            var info = RaceTemplates.templates[raceDisplay[moused.Text]];
            //stats.Clear(new Rectangle(0, 0, stats.Width, 3));
            //stats.Cursor.Position = new Point(0, 0);
            //stats.Cursor.Print(info.Info, stats.Theme.FillStyle, null);
            tooltip.Clear();
            tooltip.Cursor.Position = new Point(0, 0);
            tooltip.Cursor.Print(info.Info, races.Theme.FillStyle, null);
            tooltip.Show();
            toolOn = moused;
          } 
        };
        
        chk.MouseExit += (snd, args) =>
        {
          if ((snd as RadioButton) == toolOn)
          {
            tooltip.Hide();
          }
        };
        if (first) 
        {
          chk.IsSelected = true;
          chk.IsDirty = true;
        }
        races.Add(chk);
        counter++;
        first = false;
      }
      races.Position = new Point(RACEX, RACEY);
      tooltip.Position = new Point(TOOLX, TOOLY);
      races.Children.Add(tooltip);
      Children.Add(races);
    }

    private void InitTraits()
    {
      int traitCounter = 0;
      var traitTool = new Window(TRAITTOOLW, TRAITTOOLH);
      UIUtils.border(traitTool);
      UIUtils.border(traits, "Traits");
      foreach (var trait in TraitTemplates.templates)
      {
        traitDisplay[trait.Value.Desc] = trait.Key;
        var chk = new CheckBox(trait.Value.Desc.Length + 4, 1);
        chk.Text = trait.Value.Desc;
        chk.Position = new Point(1, traitCounter);
        chk.IsSelectedChanged += (snd, arg) =>
        {
          var clicked = snd as CheckBox;
          if (clicked != null)
          {
            stats.Clear(new Rectangle(TRAITCOL, STRROW, 8, 6));
            var traitBonuses = getTraitTotals();
            for (int j=0; j<traitBonuses.Length; j++)
            {
              stats.Print(TRAITCOL, j+STRROW, $"{traitBonuses[j], 8}");
            }
            if (clicked.IsSelected)
            {
              var toRemove = traits.Controls
                .OfType<CheckBox>()
                .Where(o => o.IsSelected)
                .AsEnumerable();
              foreach (var item in toRemove)
              {
                var info = TraitTemplates.templates[traitDisplay[item.Text]];
                if (info.Type == "profession" && info.Desc != clicked.Text)
                {
                  item.IsSelected = false;
                }
              }
              if (toRemove.Count() > 2)
              {
                clicked.IsSelected = false;
              }
            }
            UpdateAllStats();
          }
        };
        chk.MouseEnter += (snd, args) => 
        {
          var moused = snd as CheckBox;
          if (moused != null)
          {
            var mousedTrait = TraitTemplates.templates[traitDisplay[moused.Text]];
            //stats.Clear(new Rectangle(0, 3, stats.Width, 3));
            //stats.Cursor.Position = new Point(0, 3);
            //stats.Cursor.Print(mousedTrait.Info, stats.Theme.FillStyle, null);
            traitTool.Clear();
            traitTool.Cursor.Position = new Point(0, 0);
            traitTool.Cursor.Print(mousedTrait.Info, stats.Theme.FillStyle, null);
            traitTool.Show();
            toolOn = moused;
          }
        };
        chk.MouseExit += (snd, args) =>
        {
          if ((snd as CheckBox) == toolOn)
          {
            traitTool.Hide();
          }
        };
        traits.Add(chk);
        traitCounter++;
      }
      traits.Position = new Point(TRAITX, TRAITY);
      traitTool.Position = new Point(TRAITTOOLX, TRAITTOOLY);
      traits.Children.Add(traitTool);
      Children.Add(traits);
      
    }

    private void InitStats()
    {
      var spentStr = SadConsole.Controls.ScrollBar.Create(Orientation.Horizontal, 13);
      var spentStam = SadConsole.Controls.ScrollBar.Create(Orientation.Horizontal, 13);
      var spentSpd = SadConsole.Controls.ScrollBar.Create(Orientation.Horizontal, 13);
      var spentSkl = SadConsole.Controls.ScrollBar.Create(Orientation.Horizontal, 13);
      var spentSag = SadConsole.Controls.ScrollBar.Create(Orientation.Horizontal, 13);
      var spentSmt = SadConsole.Controls.ScrollBar.Create(Orientation.Horizontal, 13);
      var strLabel = new Label(2, 1);
      var stamLabel = new Label(2, 1);
      var spdLabel = new Label(2, 1);
      var sklLabel = new Label(2, 1);
      var sagLabel = new Label(2, 1);
      var smtLabel = new Label(2, 1);
      var resetBtn = new Button(7, 1)
      {
        Position = new Point(85, 0),
        Text = "Reset"
      };
      var finishBtn = new Button(12, 1)
      {
        Position = new Point(85, 2),
        Text = "Finished",
        IsEnabled = false
      };
      var randomBtn = new Button(7, 1)
      {
        Position = new Point(85, 1),
        Text = "Random"
      };

      ScrollBar[] sbs = new ScrollBar[] {
        spentStr,
        spentStam,
        spentSpd,
        spentSkl,
        spentSag,
        spentSmt
      };

      EventHandler onValChange = (object snd, EventArgs args) =>
      {
        var changed = snd as ScrollBar;
        if (changed != null)
        {
          switch (changed.Name)
          {
            case "str":
              strLabel.Caption = spentStr.Value.ToString();
              str = spentStr.Value;
              UpdateStr();
              break;
            case "stam":
              stamLabel.Caption = spentStam.Value.ToString();
              stam = spentStam.Value;
              UpdateStam();
              break;
            case "spd":
              spdLabel.Caption = spentSpd.Value.ToString();
              spd = spentSpd.Value;
              UpdateSpd();
              break;
            case "skl":
              sklLabel.Caption = spentSkl.Value.ToString();
              skl = spentSkl.Value;
              UpdateSkl();
              break;
            case "sag":
              sagLabel.Caption = spentSag.Value.ToString();
              sag = spentSag.Value;
              UpdateSag();
              break;
            case "smt":
              smtLabel.Caption = spentSmt.Value.ToString();
              smt = spentSmt.Value;
              UpdateSmt();
              break;
            default:
              break;
          }
          string toPrint = "";
          Color color = Color.Violet;
          stats.Clear(new Rectangle(0, 0, stats.Width, 1));
          if (overspent)
          {
            int over = spentCP - CP;
            string osString = UIUtils.decorate($"{over}", "yellow");
            toPrint = $"**You may only spend 30 points total. Remove {osString} {UIUtils.pluralize("point", over)} to continue.";
            finishBtn.IsEnabled = false;
          }
          else if (underspent)
          {
            int under = CP - spentCP;
            string usString = UIUtils.decorate($"{under}", "yellow");
            toPrint = $"**You have {usString} {UIUtils.pluralize("point", under)} to spend. You must spend all of your points to continue.";
            finishBtn.IsEnabled = false;
          }
          else
          {
            toPrint = "**You have spent all of your points. You may continue now.";
            color = Color.LimeGreen;
            finishBtn.IsEnabled = true;
          }
          stats.Cursor.Position = new Point(0, 0);
          stats.Cursor.Print(toPrint, new Cell(color, stats.DefaultBackground), null);
        }
      };

      spentStr.Name = "str";
      spentSkl.Name = "skl";
      spentSpd.Name = "spd";
      spentStam.Name = "stam";
      spentSag.Name = "sag";
      spentSmt.Name = "smt";

      for (int i=0; i < sbs.Length; i++)
      {
        var bar = sbs[i];
        bar.ValueChanged += onValChange;
        bar.Position = new Point(SPENTCOL + 9, STRROW + i);
        bar.Maximum = 10;
        bar.Value = 1;
        bar.Value = 0;
      }

      strLabel.Position = new Point(SPENTCOL + 6, STRROW);
      stamLabel.Position = new Point(SPENTCOL + 6,STAMROW);
      spdLabel.Position = new Point(SPENTCOL + 6, SPDROW);
      sklLabel.Position = new Point(SPENTCOL + 6, SKLROW);
      sagLabel.Position = new Point(SPENTCOL + 6, SAGROW);
      smtLabel.Position = new Point(SPENTCOL + 6, SMTROW);

      UIUtils.border(stats, "Stats");
      stats.Print(STATCOL, HEADROW, $"{"Stat",8}");
      stats.Print(TOTALCOL, HEADROW, $"{"Total",8}");
      stats.Print(RACECOL, HEADROW, $"{"Race",8}");
      stats.Print(TRAITCOL, HEADROW, $"{"Trait",8}");
      stats.Print(SPENTCOL, HEADROW, $"{"Spent",8}");
       
      var statNames = new string[]{
        "Strength",
        "Stamina",
        "Speed",
        "Skill",
        "Sagacity",
        "Smarts"
      };
      int curY;
      for (int i = 0; i<statNames.Length; i++) {
        curY = i + STRROW;
        stats.Print(STATCOL, curY, $"{statNames[i],8}");
      }

      resetBtn.Click += (object snd, EventArgs args) => 
      {
        var clicked = snd as Button;
        if (clicked != null)
        {
          foreach (var bar in sbs)
          {
            bar.Value = 0;
          }
        }
      };
      
      finishBtn.Click += (object snd, EventArgs args) =>
      {
        var clicked = snd as Button;
        if (clicked != null)
        {
          //TODO: pass results of char gen
          UIManager.setScreen("play");
        }
      };

      randomBtn.Click += (object snd, EventArgs args) =>
      {
        var clicked = snd as Button;
        var rng = World.Instance.RNG;
        var curSpent = 0;
        if (clicked != null)
        {
          int roll;
          foreach (var bar in sbs)
          {
            if (curSpent < 20)
            {
              bar.Value = 0;
              roll = rng.Next(1, 11);
              roll = Math.Min(roll, 20 - curSpent);
              curSpent += roll;
              bar.Value = roll;        
            }
            else
              bar.Value = 0;    
          }
        }
      };
      UIUtils.AddRange(
        stats, 
        spentStr,
        spentStam,
        spentSpd,
        spentSkl,
        spentSag,
        spentSmt,
        strLabel,
        stamLabel,
        spdLabel,
        sklLabel,
        sagLabel,
        smtLabel,
        resetBtn,
        randomBtn,
        finishBtn
      );
      stats.Position = new Point(STATX, STATY);
      Children.Add(stats);
    }

    private void UpdateStr()
    {
      stats.Clear(new Rectangle(TOTALCOL, STRROW, 8, 1));
      stats.Print(TOTALCOL, STRROW, $"{strTotal,8}");
    }

    private void UpdateStam()
    {
      stats.Clear(new Rectangle(TOTALCOL, STAMROW, 8, 1));
      stats.Print(TOTALCOL, STAMROW, $"{stamTotal,8}");
    }

    private void UpdateSpd()
    {
      stats.Clear(new Rectangle(TOTALCOL, SPDROW, 8, 1));
      stats.Print(TOTALCOL, SPDROW, $"{spdTotal,8}");
    }

    private void UpdateSkl()
    {
      stats.Clear(new Rectangle(TOTALCOL, SKLROW, 8, 1));
      stats.Print(TOTALCOL, SKLROW, $"{sklTotal,8}");
    }

    private void UpdateSag()
    {
      stats.Clear(new Rectangle(TOTALCOL, SAGROW, 8, 1));
      stats.Print(TOTALCOL, SAGROW, $"{sagTotal,8}");
    }    
    
    private void UpdateSmt()
    {
      stats.Clear(new Rectangle(TOTALCOL, SMTROW, 8, 1));
      stats.Print(TOTALCOL, SMTROW, $"{smtTotal,8}");
    }

    private void UpdateAllStats()
    {
      UpdateStr();
      UpdateStam();
      UpdateSkl();
      UpdateSpd();
      UpdateSag();
      UpdateSmt();
    }

    private int[] getTraitTotals()
    {
      int str = 0;
      int stam = 0;
      int sag = 0;
      int spd = 0;
      int skl = 0;
      int smt = 0;
      var selectedTraitList = traits.Controls
        .OfType<CheckBox>()
        .Where(i => i.IsSelected)
        .AsEnumerable();
      foreach (var traitBox in selectedTraitList)
      {
        var traitID = traitDisplay[traitBox.Text];
        var trait = TraitTemplates.templates[traitID];
        str += trait.BonusStats?.Str ?? 0;
        stam += trait.BonusStats?.Stam ?? 0;
        skl += trait.BonusStats?.Skl ?? 0;
        sag += trait.BonusStats?.Sag ?? 0;
        spd += trait.BonusStats?.Spd ?? 0;
        smt += trait.BonusStats?.Smt ?? 0;
      }
      return new int[]{str, stam, skl, sag, spd, smt};
    }

  }
}