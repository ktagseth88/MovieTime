CREATE VIEW [overwatch].[v_player_role_record]
	AS 
SELECT Sum(CASE 
             WHEN m.victory = 1 THEN 1 
             ELSE 0 
           END) AS wins, 
       Sum(CASE 
             WHEN m.victory = 0 THEN 1 
             ELSE 0 
           END) AS losses, 
       pmx.role role, 
       p.NAME player_name,
	   p.player_id player_id
FROM   overwatch.player_match_xref pmx 
       LEFT JOIN overwatch.match m 
              ON pmx.match_id = m.match_id 
       LEFT JOIN overwatch.player p 
              ON pmx.player_id = p.player_id 
GROUP BY pmx.role, 
          p.NAME,
		  p.player_id