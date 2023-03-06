import { Radio, FormControlLabel } from '@mui/material';
//fiks ikke i bruk???

const QuizQuestionAlternative = ({ data }) => <FormControlLabel value={data} control={<Radio />} label={data} />

export default QuizQuestionAlternative;
