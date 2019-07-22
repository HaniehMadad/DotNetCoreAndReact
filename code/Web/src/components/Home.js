import React, { Component } from 'react';
import axios from 'axios';
import S3FileUpload from 'react-s3';

import {
  fileTypes,
  getFileType
} from './fileTypes';

import { awsConfig } from '../Configurations/awsConfig';

const inistalState = {
  file: null,
  fileType: null,
  uploadedSucessfully: false,
  errorMsg: '',
  successMsg: '',
};

export class Home extends Component {
  constructor() {
    super();
    this.state = {
      ...inistalState,
    };
  }
  componentDidMount() {
    console.log('componet did mount');
    axios.get('/api/upload/createTable');
  }

  uploadFile = (event) => {
    const file = event.target.files[0];
    if (!file) {
      this.setState(inistalState);
      return;
    }
    const inputFileType = getFileType(file);
    if (!inputFileType) {
      this.setState({
        ...inistalState,
        errorMsg: `Make sure file name starts with ${fileTypes.Lp} or ${fileTypes.Tou}.`,
      });
    } else {
      this.setState({
        ...inistalState,
        file: event.target.files[0],
        fileType: inputFileType,
      });
    }
  }

  uploadFileToS3 = () => {
    S3FileUpload
      .uploadFile(this.state.file, awsConfig)
      .then((data) => {
        this.setState({
          uploadedSucessfully: true,
          successMsg: `Sucessfully uploaded the file "${this.state.file.name}" to S3. Now you can calculate the aggregated result and save in API.`,
          errorMsg: '',
        });
      })
      .catch((err) => {
        this.setState({
          uploadedSucessfully: false,
          successMsg: '',
          errorMsg: `Something went wrong uploading to S3 ${err}.`,
        });
      });
  }

  storeResult = (event) => {
    event.preventDefault();
    const reader = new FileReader();
    reader.readAsText(this.state.file);
    reader.onload = (e) => {
      const apiBaseEndpoit = this.state.fileType === fileTypes.Lp
        ? '/api/upload/uploadLp'
        : '/api/upload/uploadTou';

      axios.post(apiBaseEndpoit, e.target.result, {
        headers: {
          'Content-Type': 'text/plain',
        },
      }).then((response) => {
        this.setState({
          ...inistalState,
          successMsg: `Succesfully stored the aggregated result of file type: ${this.state.fileType}. The result includes ${response.data} records.`,
        });
      }).catch((error) => {
        this.setState({
          ...inistalState,
          errorMsg: `Something went wrong ${error.response.data}.`,
        });
      });
    };
  }

  render() {
    return (
      <div>
        <h1>Please upload your file here:</h1>
        <div style={{
          display: 'flex',
          flexDirection: 'row',
          justifyContent: 'space-around',
          padding: '20px',
        }}
        >
          <input label="upload your csv file" type="file" onChange={this.uploadFile} />
          <button type="button" disabled={!(this.state.file && !this.state.uploadedSucessfully)} onClick={this.uploadFileToS3}>Store in S3</button>
          <button type="button" disabled={!(this.state.file && this.state.uploadedSucessfully)} onClick={this.storeResult}>Calculate and store in API</button>
          <div stlye={{ flex: '3' }} />
        </div>
        <div style={{ color: 'green' }}>{this.state.successMsg}</div>
        <div style={{ color: 'red' }}>{this.state.errorMsg}</div>
      </div>
    );
  }
}
