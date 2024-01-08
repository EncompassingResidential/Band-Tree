# Using BASH shell to run individual queries
#
# These commands each create their own connection to the DB.
# look in script EdgeDB command script.ql
# which only uses 1 connection.
# each query command took 2 to 4 seconds, total script took 10 seconds
#    VS cat EdgeDB\ command\ script.ql | EdgeDB
#    which took a total of 3 seconds

# you have to make sure you're connecting to the right database before running these.
# one example from command line:
# edgedb -d Band_Tree_DB
# I don't know if that works for this BASH example.
# Just check the edgedb.toml for database name

date
edgedb query "SELECT MusicCompanyLabel { name, location, foundingdate, countryoforigin, wikipediapageid, timestamp, id } ;"
date
edgedb query "SELECT Artist { artistname, birthdate, othernames, genres, labels, familyname, wikipediapageid, timestamp, id } ;"
date
edgedb query "SELECT Genre { name, wikipediapageid, timestamp, id } ;"
date
edgedb query "SELECT BandModel { title, currentmembers, pastmembers, genres, labels, wikipediapageid, wikipediapagetimestamp, id } ;"
date 

# command and output
# $ date; sh ./BandTree.Server/Doc/BandTree.EdgeDB.SELECT.queries.sql; date
# Sun, Jan  7, 2024  5:16:23 PM
# Sun, Jan  7, 2024  5:16:23 PM
# Connecting to EdgeDB instance 'Band_Tree_DB' at localhost:10703...
# {
#   "name": "Mercury Records",
#   "location": {"city": "New York City", "state": "New York", "country": "United States of America"},
#   "foundingdate": "1945-01-01T00:00:00+00:00",
#   "countryoforigin": "United States of America",
#   "wikipediapageid": 325909,
#   "timestamp": "2023-12-23T05:56:00+00:00",
#   "id": "c6ace604-ad04-11ee-8e02-dfd1c4664cb4"
# }
# Sun, Jan  7, 2024  5:16:26 PM
# Connecting to EdgeDB instance 'Band_Tree_DB' at localhost:10703...
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
# Sun, Jan  7, 2024  5:16:28 PM
# Connecting to EdgeDB instance 'Band_Tree_DB' at localhost:10703...
# {
#   "name": "Progressive Rock",
#   "wikipediapageid": 51503,
#   "timestamp": "2023-12-26T17:13:00+00:00",
#   "id": "73639556-ace1-11ee-8cf5-d3a2f4f16760"
# }
# Sun, Jan  7, 2024  5:16:30 PM
# Connecting to EdgeDB instance 'Band_Tree_DB' at localhost:10703...
# {
#   "title": "Rush (band)",
#   "currentmembers": [{"id": "736419b8-ace1-11ee-8cf5-efb9dbcea8dd"}],
#   "pastmembers": [],
#   "genres": [{"id": "73639556-ace1-11ee-8cf5-d3a2f4f16760"}],
#   "labels": [{"id": "c6ace604-ad04-11ee-8e02-dfd1c4664cb4"}],
#   "wikipediapageid": 124802,
#   "wikipediapagetimestamp": "2024-01-05T23:30:00+00:00",
#   "id": "6f6e3858-ad17-11ee-b3b0-3fd2dffd248e"
# }
# Sun, Jan  7, 2024  5:16:33 PM
# Sun, Jan  7, 2024  5:16:33 PM