# Using BASH shell to run individual queries
#
# These commands each create their own connection to the DB.
# look in script EdgeDB command script.ql
# which only uses 1 connection.

date
edgedb query 'insert default::BandModel { title := "Rush (band)",  wikipediapageid := 25432, currentmembers := (SELECT Artist FILTER .name IN {"Geddy Lee"}), genres := (SELECT Genre FILTER .name IN {"Progressive Rock"}), labels := (SELECT MusicCompanyLabel FILTER .name = "Mercury Records"), wikipediapagetimestamp := <datetime>"2024-01-05T23:30:00+00:00" };'
date
edgedb query "SELECT BandModel { title, currentmembers, pastmembers, genres, labels, wikipediapageid, wikipediapagetimestamp, id } ;"
date 

# command and output
# $ sh ./BandTree.EdgeDB.queries.sql
# Connecting to EdgeDB instance at localhost:10703...
# Connecting to EdgeDB instance at localhost:10703...
# {
#   "artistname": "Geddy Lee",
#   "birthdate": null,
#   "othernames": null,
#   "genres": [{"id": "73639556-ace1-11ee-8cf5-d3a2f4f16760"}],
#   "labels": [],
#   "familyname": null,
#   "wikipediapageid": 12964,
#   "timestamp": "2024-01-03T09:44:00+00:00",
#   "id": "736419b8-ace1-11ee-8cf5-efb9dbcea8dd"
# }
# Connecting to EdgeDB instance at localhost:10703...
# {
#   "name": "Progressive Rock",
#   "wikipediapageid": 51503,
#   "timestamp": "2023-12-26T17:13:00+00:00",
#   "id": "73639556-ace1-11ee-8cf5-d3a2f4f16760"
# }
# Connecting to EdgeDB instance at localhost:10703...
# 