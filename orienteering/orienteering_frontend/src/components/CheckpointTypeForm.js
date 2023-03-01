import { useRadioGroup } from '@mui/material/RadioGroup';
import { Radio, FormControl, FormLabel, RadioGroup, FormControlLabel, Select, MenuItem } from '@mui/material';
import React, { useState } from 'react';
//Kilder: https://mui.com/material-ui/react-radio-button/ (17.02.2023)


export default function CheckpointTypeForm() {

    const [showForm, setShowForm] = useState(false);
    const handleChange = (event) => {
        //setCheckpointInfo({ ...checkpointInfo, [event.target.name]: event.target.value });
        if (event.target.value === "spill") {
            setShowForm(true);
        }
        else {
            setShowForm(false);
        }
    };

    return<>
        <FormControl>
            <FormLabel id="radio-buttons-group">Velg aktivitet</FormLabel>
            <RadioGroup
                aria-labelledby="radio-buttons-group"
                name="controlled-radio-buttons-group"
                row
                onChange={(e) => handleChange(e)}
            >
                <FormControlLabel value="spill" control={<Radio />} label="Spill" />
                <FormControlLabel value="quiz" control={<Radio />} label="Quiz" />
            </RadioGroup>
            <FormLabel style={showForm ? {} : { display: 'none' }} id="radio-buttons-group">Velg spill</FormLabel>
            <Select
                labelId="simple-select-label"
                id="simple-select"
                onChange={handleChange}
            >
                <MenuItem value={10}>Ten</MenuItem>
                <MenuItem value={20}>Twenty</MenuItem>
                <MenuItem value={30}>Thirty</MenuItem>
            </Select>
        </FormControl>
        </>
    
}