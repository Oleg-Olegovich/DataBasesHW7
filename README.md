# DataBasesHW7
Решение домашней работы №7.

Студент: Манжула Олег.

Группа: БПИ206.

При локальных проблемах запуска, вероятно, стоит посмотреть метод</br>
DataBasesHW7.Database.AppContext.OnConfiguring


# **№1**
Для Олимпийских игр 2004 года сгенерируйте список (год рождения, количество игроков, количество золотых медалей), содержащий годы, в которые родились игроки, количество игроков, родившихся в каждый из этих лет, которые выиграли по крайней мере одну золотую медаль, и количество золотых медалей, завоеванных игроками, родившимися в этом году.

SELECT<br/>
EXTRACT(YEAR FROM players.birthdate) as birth_year,<br/>
COUNT(DISTINCT players.player_id) as players_count,<br/>
COUNT(results.medal) as gold_medals_count<br/>
FROM olympics<br/>
JOIN events ON olympics.olympic_id = events.olympic_id<br/>
JOIN results ON events.event_id = results.event_id<br/>
JOIN players ON results.player_id = players.player_id<br/>
WHERE results.medal = 'GOLD' AND year = 2004<br/>
GROUP BY birth_year;<br/>


# **№2**
Перечислите все индивидуальные (не групповые) соревнования, в которых была ничья в счете, и два или более игрока выиграли золотую медаль.

SELECT events.name<br/>
FROM events<br/>
JOIN results ON events.event_id = results.event_id<br/>
WHERE results.medal = 'GOLD' AND is_team_event = 0<br/>
GROUP BY results.result, events.name<br/>
HAVING COUNT(results.medal) > 1;<br/>


# **№3**
Найдите всех игроков, которые выиграли хотя бы одну медаль (GOLD, SILVER и BRONZE) на одной Олимпиаде. (player-name, olympic-id).

SELECT DISTINCT players.name, events.olympic_id<br/>
FROM players<br/>
`         `JOIN results ON players.player_id = results.player_id<br/>
`         `JOIN events ON results.event_id = events.event_id;<br/>


# **№4**
В какой стране был наибольший процент игроков (из перечисленных в наборе данных), чьи имена начинались с гласной?

WITH temp as (SELECT c1.country_id, cast(c2.num_players as float) / c1.num_players as ratio)<br/>
`    `FROM (SELECT country_id, count(player_id) as num_players from players group by country_id) c1,<br/>
`    ` `    `(SELECT country_id, count(player_id) as num_players)<br/>
`    ` `    ` FROM players<br/>
`    ` `    ` WHERE SUBSTR(name, 1, 1)<br/>
`    ` `    ` `    `in ('A','E','I','O','U', 'Y')<br/>
`    ` `    ` GROUP BY country_id) c2<br/>
`    `WHERE c1.country_id = c2.country_id)<br/>
SELECT c.name<br/>
FROM temp t,<br/>
`    `countries c<br/>
WHERE ratio = (SELECT MAX(ratio) FROM temp)<br/>
`    `AND t.country_id = c.country_id<br/>


# **№5**
` `Для Олимпийских игр 2000 года найдите 5 стран с минимальным соотношением количества групповых медалей к численности населения.

SELECT countries.country_id, COUNT(results.medal) \* 1.0 / population as medal_percentage<br/>
FROM olympics<br/>
`         `JOIN events ON olympics.olympic_id = events.olympic_id<br/>
`         `JOIN results ON events.event_id = results.event_id<br/>
`         `JOIN players ON results.player_id = players.player_id<br/>
`         `JOIN countries ON players.country_id = countries.country_id<br/>
WHERE is_team_event = 1 AND year = 2000<br/>
GROUP BY countries.country_id, population<br/>
ORDER BY medal_percentage LIMIT 5;<br/>
