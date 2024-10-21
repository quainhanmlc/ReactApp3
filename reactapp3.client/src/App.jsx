import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import ForgotPassword from './components/ForgotPassword';
import ResetPassword from './components/ResetPassword';
import ChangePassword from './components/ChangePassword';
import ConfirmEmail from './components/ConfirmEmail';
import TeamList from './components/TeamList';
import TeamDetail from './components/TeamDetail';
import CreateProject from './components/CreateProject';
import CreateTeam from './components/CreateTeam';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Navigate to="/login" />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/forgot-password" element={<ForgotPassword />} />
                <Route path="/confirm-email/:email" element={<ConfirmEmail />} />
                <Route path="/reset-password/:token" element={<ResetPassword />} />
                <Route path="/change-password" element={<ChangePassword />} />
                <Route path="/teams" element={<TeamList />} />
                <Route path="/teams/:id" element={<TeamDetail />} />
                <Route path="/create-project" element={<CreateProject />} />
                <Route path="/create-team" element={<CreateTeam />} />
            </Routes>
        </Router>
    );
}

export default App;
