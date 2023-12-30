module default {
    scalar type State extending enum<NotStarted, InProgress, Complete>;

    type Artists {
        required property ArtistName -> str {
            constraint exclusive;
            constraint min_len_value(8);
        }
        required property Origin -> str;
        required property date_created -> std::datetime {
            default := std::datetime_current();
        }
        required property MemberOf -> str;
		property WikipediaURL -> str;
		property ArtistURL -> str;
    }
	
	type Band {
		required property BandName -> str {
            constraint exclusive;
            constraint min_len_value(8);
        }
		property WikipediaURL -> str;
		property BandURL -> str;
	}

	type Genre {
		required property name -> str {
            constraint exclusive;
            constraint min_len_value(8);
        }
		property WikipediaURL -> str;
	}
}
