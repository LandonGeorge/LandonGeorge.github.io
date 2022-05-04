const main = async () => {
  //class that generates instances of the Transactions contract
  const Transactions = await hre.ethers.getContractFactory("Transactions");
  const transactions = await Transactions.deploy();

  await transactions.deployed();

  console.log("Transactions deployed to:", transactions.address);
}

const runMain = async () => {
  try{
    //executes and deploys our contract
    await main();
    process.exit(0);
  }catch(error){
    console.error(error);
    process.exit(1);
  }
}

//this is ran first in this file
runMain();
