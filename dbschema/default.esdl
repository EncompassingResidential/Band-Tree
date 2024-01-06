module default {
	type BandModel {
		required                        title: str {
									constraint exclusive;
									constraint min_len_value(3);
								}
		required              wikipediapageid: int32;
		required multi         currentmembers: Artist;
		         multi            pastmembers: Artist;
		required multi                 genres: Genre;
		         multi                 labels: MusicCompanyLabel;
		required       wikipediapagetimestamp: datetime;
	}

	type Artist {
		required                   artistname: str {
									constraint exclusive;
									constraint min_len_value(3);
								}
		                           othernames: array<str>;
		required              wikipediapageid: int32;
		                            birthdate: datetime;
		                           familyname: tuple<first: str, middle: str, last: str>;
		required multi                 genres: Genre;
		         multi                 labels: MusicCompanyLabel;
		required                    timestamp: datetime;
		}

	type Genre {
		required                         name: str {
									constraint exclusive;
									constraint min_len_value(3);
								}
		required              wikipediapageid: int32;
		required                    timestamp: datetime;
	}

	type MusicCompanyLabel {
		required                         name: str {
									constraint exclusive;
									constraint min_len_value(3);
								}
		required              wikipediapageid: int32;
		                         foundingdate: datetime;
		                      countryoforigin: str;
		                             location: tuple<city: str, state: str, country: str>;
		required                    timestamp: datetime;
	}
};