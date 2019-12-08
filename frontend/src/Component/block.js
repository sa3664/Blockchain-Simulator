import React from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {  faCross, faPlusCircle, faWindowClose } from '@fortawesome/free-solid-svg-icons'
import { getHashes } from 'crypto';

class Block extends React.Component {
    constructor() {
        super();
        this.state = {
             transactions: [],
            blockdata: '',
            tsender : ' ',
            trecipient : '',
            tamount : '',
            blockhash: '',
            isLoading: true,
            color:'has-background-success'
        }
        this.handlechange = this.handlechange.bind(this);
    }

    handlechange(state, event) {
        //this.setState({})
        console.log(event.target.value)
        this.setState({
         state: event.target.value
        });
      }

      handlesender(event) {
        //this.setState({})
        console.log(event.target.value)
        this.setState({
         tsender: event.target.value
        });
      }

      handlerecipient(event) {
        //this.setState({})
        console.log(event.target.value)
        this.setState({
         trecipient: event.target.value
        });
      }
      handleamount(event) {
        //this.setState({})
        console.log(event.target.value)
        this.setState({
         tamount: event.target.value
        });
      }

      getTHash(trxs){
        event.preventDefault();
        fetch('https://localhost:44319/api/block/getTHash', {
            method: 'post',
            headers: {'Content-Type':'application/json'},
            body: JSON.stringify(trxs)
          })
          .then(response => { return response.json() })
          .then(data => this.setState({blockhash: data}));
      }

    removeTransaction(tHash){
        this.setState({color:"has-background-danger"});
        console.log(this.state.transactions)
        var trx = this.state.transactions;
        var rtrx = trx.find( t => t.TransactionHash == tHash );
        console.log(rtrx);
        var index = trx.indexOf(rtrx);
        console.log(index);
        trx.splice(index, 1);
        this.setState({transactions:trx},()=> this.getTHash(this.state.transactions));
    }

    AddTransaction(){
        this.setState({color:"has-background-danger"});
        var trx = {};
        trx.Sender = this.state.tsender;
        trx.Recipient = this.state.trecipient;
        trx.Amount = this.state.tamount;
        trx.TransactionHash = Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
        console.log(trx);
        this.setState({transactions:[...this.state.transactions, trx]}, ()=>  this.getTHash(this.state.transactions));
        this.setState({tsender:"", trecipient:"", tamount:""});       
    }

    componentDidMount() {
        fetch('https://localhost:44319/api/block/mine')
            .then(response => { return response.json() })
            //  .then(data => console.log({data}))
            .then(data => this.setState({ blockdata: JSON.parse(data), isLoading: false, transactions: JSON.parse(data).Transactions  }));

        console.log("blockdata");
        console.log(this.state);
    }

    mineBlock()
    {
        event.preventDefault();
        fetch('https://localhost:44319/api/block/mine', {
            method: 'post',
            headers: {'Content-Type':'application/json'},
            body: JSON.stringify(this.state.transactions)
          })
          .then(response => { return response.json() })
          //  .then(data => console.log({data}))
          .then(data => this.setState({ blockdata: JSON.parse(data), isLoading: false, transactions: JSON.parse(data).Transactions, color: "has-background-success"  }));
       //   this.setState({transactions:trx});
    }
    render() {
        { console.log(this.state) }
        return (
            <div className={`box  ${this.state.color}`}>
            <form >
                <div className="field">
                    <label className="label">Block</label>
                    <div className="control">
                        <input
                            className="input"
                            type="text"
                            placeholder=" .. "
                            value={this.state.blockdata.Index}
                        //  onChange={(e) => this.setState({ monthlyIncome: e.target.value })}
                        />
                    </div>
                </div>
                <div className="field">
                    <label className="label">Parent</label>
                    <div className="control">
                        <input
                            className="input"
                            type="text"
                            placeholder="..."
                            value={this.state.blockdata.PreviousHash}
                        />
                    </div>
                </div>
                <div className="field">
                    <label className="label">Nonce</label>
                    <div className="control">
                        <input
                            className="input"
                            type="text"
                            placeholder="..."
                            value={this.state.blockdata.Nonce}
                         //   onChange={(e) => this.setState({ monthlyExpenses: e.target.value })}
                        />
                    </div>
                </div>
                <div className="field">
                    <label className="label">Hash</label>
                    <div className="control">
                        <input
                            className="input"
                            type="text"
                            placeholder="..."
                            value={this.state.blockhash != '' ? this.state.blockhash : this.state.blockdata.Hash}
                          //  onChange={(e) => this.setState({ monthlyExpenses: e.target.value })}
                        />
                    </div>
                </div>
                <div className="field">
                    <label className="label">Transaction</label>
                    <div className="control">
                        <table className="table" id="transactionTbl">
                            <thead>
                                <tr>
                                    <th>Sender</th>
                                    <th>Recipient</th>
                                    <th>Amount</th>
                                    <th> </th>
                                </tr>

                                </thead>
                                {!this.state.isLoading ? this.state.transactions.map((row) => {
                                    return <tr key={row.TransactionHash}><td>{row.Sender}</td><td>{row.Recipient}</td><td>{row.Amount}</td><td>  <span className="icon has-text-info is-medium" onClick={()=> this.removeTransaction(row.TransactionHash)}><FontAwesomeIcon icon={faWindowClose} size="sm" /></span></td></tr>
                                }
                                ) : null}

                <tr>
                    <td>  <input id="Sender" value={this.state.tsender} onChange={ ()=> this.handlesender(event)} className="input" type="text"></input> </td>
                    <td>  <input id="Recipient" value={this.state.trecipient} onChange={() => this.handlerecipient(event)} className="input" type="text"></input> </td>
                    <td>  <input id="Amount" value={this.state.tamount} onChange={() => this.handleamount(event)} className="input" type="text"></input> </td>
                    <td>  <span className="icon has-text-info is-medium" onClick={()=> this.AddTransaction()}><FontAwesomeIcon icon={faPlusCircle} size="md" /></span>
                    </td>
                </tr>
                             


                        </table>
                    </div>
                </div>
                <button className="button is-warning is-medium" onClick={()=>this.mineBlock()} >Mine</button>
                <br />
                <br />
            </form>
</div>
        )
    }
}

export default Block;