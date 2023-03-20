import { Radio, FormControlLabel } from '@mui/material';


const QuizQuestionAlternative = ({ data }) => <FormControlLabel value={data} control={<Radio />} label={data} />

export default QuizQuestionAlternative;
