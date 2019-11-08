var RubiToken = artifacts.require("RubiToken.sol");
const _name = "Rubi";
const _symbol = "LCS";
const _decimals = 18;
const _total_supply = 1000000;
module.exports = function(deployer) {
    deployer.deploy(RubiToken, _name, _symbol, _decimals, _total_supply);
};