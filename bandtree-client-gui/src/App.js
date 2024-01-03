import logo from './logo.svg';
import './App.css';
import WikipediaSearch from './BandTree_Client_WikipediaSearch';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React John again here it is 1/02/2024 14:52
        </a>
        John Text @ 15:16 <WikipediaSearch />
      </header>
    </div>
  );
}

export default App;
