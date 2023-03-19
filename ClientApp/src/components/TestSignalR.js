import React, { Component }from 'react';
import { HubConnectionBuilder } from "@microsoft/signalr";

export class TestSignalR extends Component{
    static displayName = 'TestSignalR';

    constructor(props){
        super(props);
        this.state = { category : [], loading : true}

        //SignalR
        const connection = new HubConnectionBuilder()
        .withUrl("https://localhost:44371/api/category")
        .build();

        connection.on("CategoryListUpdated", (categories) => {
            // Cập nhật danh sách Category ở đây
        });

        connection.start();
    
    }
    
    
    
    componentDidMount(){
        this.categoryData();
    }
    
    static renderCategoryTables(category) {
        console.log(category);
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Content</th>
            <th>Add Date</th>
          </tr>
        </thead>
        <tbody>
          {category.map(data =>
            <tr key={data.id}>
              <td>{data.id}</td>
              <td>{data.name}</td>
              <td>{data.content}</td>
              <td>{data.addedDate}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render(){
        
    let contents = this.state.loading ? <p>Loading...</p> : TestSignalR.renderCategoryTables(this.state.category);
    return (
        <div>
            <h1 id="tabelLabel" >Category with SignalR</h1>
            <p>This component demonstrates fetching data from the server and using SignalR.</p>
            {contents}
        </div>
    )
  }
    async categoryData(){
        const response = await fetch('https://localhost:44371/api/category');
        const data = await response.json();
        this.setState({ category : data, loading : false });
    }


}

