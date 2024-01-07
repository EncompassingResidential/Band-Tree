import logo from './logo.svg';
import './App.css';
import React, { useState } from 'react';
import WikipediaSearch from './BandTree_Client_WikipediaSearch';
import SearchWikiResultsList from './SearchWikiResultsList';

function App() {
  const [searchJSONResults, setSearchResults] = useState([]);

  const handleSearchResults = (data) => {
    setSearchResults(data);
  };

  return (
    <div className="App">
      <header className="App-header">
        <img src=".\3d-force-graph example 3D graph - detail.png" className="Band-Tree" alt="3D Graph John" />
        <p>This is the Band Tree Client
        <br />
          John learning React again, here it is 1/02/2024 14:52
        </p>
      </header>
      <hr />
      calling WikipediaSearch here <WikipediaSearch onResultsFetched={handleSearchResults}/>
      <hr />
      <hr />
      calling SearchWikiResultsList here 1/03/24 19:27 <SearchWikiResultsList searchWikiResults={searchJSONResults} />
    </div>
  );
}

export default App;
