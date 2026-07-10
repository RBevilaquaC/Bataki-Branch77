using SQLite;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

[Table("PlayerData")]
public class PlayerData
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Column("player")]
    public string Player { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("company_name")]
    public string CompanyName { get; set; }
}

[Table("ScoreBoard")]
public class ScoreBoard
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    // ID do jogador da tabela PlayerData
    [Column("player_id")]
    public int PlayerId { get; set; }

    [Column("time")]
    public System.DateTime Time { get; set; }

    [Column("score")]
    public int Score { get; set; }
}


public class DataManager : MonoBehaviour
{   
    SQLiteConnection db;

    void Start()
    {
        string path = Path.Combine(Application.persistentDataPath, "bataki77DB.db");

        db = new SQLiteConnection(path);

        db.CreateTable<PlayerData>();

        db.CreateTable<ScoreBoard>();
        
    }

    public void InsertPlayerData(string player, string email, string companyName)
    {
        var existingPlayer = db.Table<PlayerData>().FirstOrDefault(p => p.Player == player);

        GameController.gc.playerName = player;

        if (existingPlayer == null)
        {
            var playerData = new PlayerData 
            { 
                Player = player, 
                Email = email, 
                CompanyName = companyName 
            }; 

            db.Insert(playerData);
        }
        else
        {
            existingPlayer.Email = email;
            existingPlayer.CompanyName = companyName;

            db.Update(existingPlayer);
        }
    }

    public void InsertScore(string playerName, int score)
    {
        var player = db.Table<PlayerData>()
                    .FirstOrDefault(p => p.Player == playerName);

        var scoreBoard = new ScoreBoard
        {
            PlayerId = player.Id,
            Time = System.DateTime.Now,
            Score = score
        };

        db.Insert(scoreBoard);
    }

    public List<(string Player, int Score)> GetTop4Players()
    {
        string sql = @"
        SELECT *
        FROM ScoreBoard player
        WHERE player.id IN
        (
            SELECT id
            FROM ScoreBoard
            WHERE player_id = player.player_id
            ORDER BY score DESC, time ASC
            LIMIT 1
        )
        ORDER BY score DESC
        LIMIT 4;
        ";

        return db.Query<ScoreBoard>(sql)
             .Select(s => (db.Find<PlayerData>(s.PlayerId)?.Player ?? "Unknown", s.Score))
             .ToList();
    }

}