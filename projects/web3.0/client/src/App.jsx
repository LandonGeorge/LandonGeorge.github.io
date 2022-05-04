import { Navbar, Welcome, Footer, Services, Transactions } from './components';
const App = () => {
  return (
    <div className="min-h-screen">
      <div className="gradient-bg-welcome">
        {/*references the Navbar component file ctrl + click to go to file*/}
        <Navbar />
        <Welcome />
      </div>
      <Services />
      <Transactions />
      <Footer />
    </div>
  )
}

export default App
