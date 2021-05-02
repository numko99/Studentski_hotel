import React,{ useState, useEffect } from 'react'
import { makeStyles } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import './PrikazZaposlenih.css'
import axios from 'axios'

const useStyles = makeStyles({
    table: {
      width: 800,
    },
  });



const PrikazZaposlenih=()=>{
  const [Data, setData] = useState({Uposlenici:[]});
  const classes = useStyles();
  useEffect(() => { 

    axios.get("https://localhost:44328/AdminApi/PrikazOsoblja").then(result=>(
    setData({Uposlenici:result.data})
    ));
    

},[]);
  return (
      <div className='PrikazZaposlenih'>
          <div>
    <TableContainer component={Paper}>
      <Table className={classes.table} aria-label="simple table">
        <TableHead>
          <TableRow>
            <TableCell>Ime</TableCell>
            <TableCell align="right">Prezime</TableCell>
            <TableCell align="right">Pozicija</TableCell>
            <TableCell align="right">Datum Zaposlenja</TableCell>
            <TableCell align="right"></TableCell>

          </TableRow>
        </TableHead>
        <TableBody>
          {Data.Uposlenici.map((row) => (
            <TableRow key={row.id}>
              <TableCell component="th" scope="row">
                {row.ime}
              </TableCell>
              <TableCell align="right">{row.prezime}</TableCell>
              <TableCell align="right">Referent</TableCell>
              <TableCell align="right">{row.datumRodjenja}</TableCell>
              <TableCell align="right"><button>Uredi</button></TableCell>

            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
    </div>
    </div>
  );
}
export default PrikazZaposlenih;