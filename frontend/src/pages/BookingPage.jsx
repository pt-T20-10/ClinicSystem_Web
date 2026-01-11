import React, { useState, useEffect } from 'react';
import { Layout, Steps, Card, Form, Select, DatePicker, Button, message, Row, Col, Divider, Tag, Input, Typography } from 'antd'; // Nhớ import Typography
import { UserOutlined, CalendarOutlined, SolutionOutlined } from '@ant-design/icons';
import { useLocation, useNavigate } from 'react-router-dom';
import dayjs from 'dayjs';
import axios from '../services/axios';

const { Title, Text } = Typography;
const { Option } = Select;
const { TextArea } = Input;

// Hàm Helper: Parse chuỗi lịch làm việc

const BookingPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const [currentStep, setCurrentStep] = useState(0);
  
  // Dữ liệu
  const [doctors, setDoctors] = useState([]);
  const [services, setServices] = useState([]);
  const [bookedSlots, setBookedSlots] = useState([]);

  // --- SỬA LỖI 1: Khởi tạo giá trị ban đầu trực tiếp (Lazy Initialization) ---
  const [selectedDoctor, setSelectedDoctor] = useState(() => {
     return location.state?.doctor || null;
  });

  // Form State
  const [selectedService, setSelectedService] = useState(null);
  const [selectedDate, setSelectedDate] = useState(null);
  const [selectedTime, setSelectedTime] = useState(null);
  const [patientInfo, setPatientInfo] = useState({ name: '', phone: '', note: '' });

  // Load danh sách Bác sĩ & Dịch vụ
  useEffect(() => {
    const fetchData = async () => {
      try {
        const [docRes, serviceRes] = await Promise.all([
          axios.get('/Doctor'),
          axios.get('/Booking/types') 
        ]);
        setDoctors(docRes);
        setServices(serviceRes);
      } catch {
        message.error("Lỗi tải dữ liệu");
      }
    };
    fetchData();
  }, []);


  useEffect(() => {
    if (selectedDoctor && selectedDate) {
      const fetchBookedSlots = async () => {
        try {
            const res = await axios.get(`/Doctor/booked-slots`, {
                params: {
                    doctorId: selectedDoctor.maNhanVien,
                    date: selectedDate.format('YYYY-MM-DD')
                }
            });
            const formattedSlots = res.map(t => t.substring(0, 5)); 
            setBookedSlots(formattedSlots);
        } catch (error) {
            console.error(error);
        }
      };
      fetchBookedSlots();
    }
    // Không cần else { setBookedSlots([]) } ở đây nữa, ta xử lý ở onChange
  }, [selectedDoctor, selectedDate]);


  // --- LOGIC UI ---
const disabledDate = (current) => {
    // 1. Chặn quá khứ
    if (current && current < dayjs().endOf('day').subtract(1, 'day')) return true;

    // 2. Chặn ngày nghỉ (Dùng dữ liệu workingDays từ Backend gửi sang)
    if (selectedDoctor && selectedDoctor.workingDays) {
        const currentDayOfWeek = current.day(); // 0 (CN) -> 6 (T7)
        // Nếu ngày hiện tại KHÔNG nằm trong danh sách làm việc -> Disable
        return !selectedDoctor.workingDays.includes(currentDayOfWeek);
    }
    return false;
};

  const generateTimeSlots = () => {
    const slots = [];
    let startHour = 7; 
    let endHour = 17;

    for (let h = startHour; h < endHour; h++) {
        slots.push(`${h.toString().padStart(2, '0')}:00`);
        slots.push(`${h.toString().padStart(2, '0')}:30`);
    }
    return slots;
  };

  const handleBooking = async () => {
    if(!selectedService || !selectedDate || !selectedTime) {
        message.error("Vui lòng chọn đầy đủ thông tin!");
        return;
    }

    const user = JSON.parse(localStorage.getItem('user'));
    if(!user) {
        message.warning("Vui lòng đăng nhập để đặt lịch!");
        navigate('/login');
        return;
    }

    try {
        const finalDateTime = selectedDate.format('YYYY-MM-DD') + 'T' + selectedTime + ':00';
        const payload = {
            maBenhNhan: user.maBenhNhan, 
            maBacSi: selectedDoctor ? selectedDoctor.maNhanVien : null,
            maLoaiLichHen: selectedService,
            ngayHen: finalDateTime,
            ghiChu: patientInfo.note
        };

        await axios.post('/Booking', payload);
        message.success("Đặt lịch thành công!");
        navigate('/'); 
    } catch (error) {
        message.error(error.response?.data?.message || "Đặt lịch thất bại");
    }
  };

  const renderStepContent = () => {
    if (currentStep === 0) {
        return (
            <Card title="Bước 1: Chọn thông tin khám" bordered={false}>
                <Form layout="vertical">
                    <Form.Item label="Chọn Dịch vụ / Chuyên khoa" required>
                        <Select 
                            placeholder="Chọn loại khám (VD: Khám tổng quát)" 
                            value={selectedService}
                            onChange={val => setSelectedService(val)}
                        >
                            {services.map(s => (
                                <Option key={s.maLoaiLichHen} value={s.maLoaiLichHen}>
                                    {s.tenLoai}
                                </Option>
                            ))}
                        </Select>
                    </Form.Item>

                    <Form.Item label="Chọn Bác sĩ (Không bắt buộc)">
                        <Select
                            placeholder="Chọn bác sĩ mong muốn"
                            allowClear
                            value={selectedDoctor?.maNhanVien}
                            onChange={(val) => {
                                const doc = doctors.find(d => d.maNhanVien === val);
                                setSelectedDoctor(doc || null);
                                setSelectedDate(null); 
                                // Reset danh sách giờ đã đặt ngay khi đổi bác sĩ
                                setBookedSlots([]); 
                            }}
                        >
                            {doctors.map(d => (
                                <Option key={d.maNhanVien} value={d.maNhanVien}>
                                    {d.hoTen} - {d.chuyenKhoa}
                                </Option>
                            ))}
                        </Select>
                        {selectedDoctor && (
                            <div style={{ marginTop: 10, padding: 10, background: '#f5f5f5', borderRadius: 6 }}>
                                <Text strong>Lịch làm việc:</Text> {selectedDoctor.lichLamViec}
                            </div>
                        )}
                    </Form.Item>
                </Form>
                <Button type="primary" onClick={() => {
                    if(!selectedService) return message.error("Vui lòng chọn dịch vụ!");
                    setCurrentStep(1);
                }}>Tiếp theo</Button>
            </Card>
        );
    }

    if (currentStep === 1) {
        return (
            <Card title="Bước 2: Chọn thời gian" bordered={false}>
                <Row gutter={32}>
                    <Col span={12}>
                        <Text strong display="block">1. Chọn ngày:</Text>
                        <div style={{ marginTop: 10 }}>
                            <DatePicker 
                                style={{ width: '100%' }} 
                                disabledDate={disabledDate} 
                                value={selectedDate}
                                onChange={val => {
                                    setSelectedDate(val);
                                    setSelectedTime(null);
                                    // Reset danh sách giờ đã đặt ngay khi đổi ngày
                                    setBookedSlots([]); 
                                }}
                            />
                        </div>
                    </Col>
                    <Col span={12}>
                        <Text strong display="block">2. Chọn giờ:</Text>
                        <div style={{ marginTop: 10, display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '10px' }}>
                            {selectedDate ? generateTimeSlots().map(slot => {
                                const isBooked = bookedSlots.includes(slot);
                                const isSelected = selectedTime === slot;
                                
                                return (
                                    <Button 
                                        key={slot}
                                        type={isSelected ? "primary" : "default"}
                                        disabled={isBooked} 
                                        onClick={() => setSelectedTime(slot)}
                                        style={isBooked ? { background: '#f5f5f5', color: '#ccc' } : {}}
                                    >
                                        {slot}
                                    </Button>
                                )
                            }) : <Text type="secondary">Vui lòng chọn ngày trước</Text>}
                        </div>
                        {selectedDoctor && <Text type="secondary" style={{fontSize: 12, marginTop: 5}}>* Các khung giờ màu xám là bác sĩ bận hoặc đã có lịch.</Text>}
                    </Col>
                </Row>
                <div style={{ marginTop: 20 }}>
                     <Button style={{ marginRight: 10 }} onClick={() => setCurrentStep(0)}>Quay lại</Button>
                     <Button type="primary" onClick={() => {
                         if(!selectedDate || !selectedTime) return message.error("Vui lòng chọn ngày giờ!");
                         setCurrentStep(2);
                     }}>Tiếp theo</Button>
                </div>
            </Card>
        );
    }

    return (
        <Card title="Bước 3: Xác nhận thông tin" bordered={false}>
            <Text strong>Dịch vụ:</Text> {services.find(s=>s.maLoaiLichHen === selectedService)?.tenLoai} <br/>
            <Text strong>Bác sĩ:</Text> {selectedDoctor ? selectedDoctor.hoTen : "Bác sĩ ngẫu nhiên"} <br/>
            <Text strong>Thời gian:</Text> <Tag color="blue">{selectedTime} - {selectedDate?.format('DD/MM/YYYY')}</Tag> <br/>
            
            <Divider />
            <Form layout="vertical">
                <Form.Item label="Ghi chú / Triệu chứng (nếu có)">
                    <TextArea rows={4} onChange={e => setPatientInfo({...patientInfo, note: e.target.value})} />
                </Form.Item>
            </Form>

            <div style={{ marginTop: 20 }}>
                <Button style={{ marginRight: 10 }} onClick={() => setCurrentStep(1)}>Quay lại</Button>
                <Button type="primary" size="large" onClick={handleBooking}>XÁC NHẬN ĐẶT LỊCH</Button>
            </div>
        </Card>
    );
  };

  return (
    <div style={{ maxWidth: 1000, margin: '40px auto', padding: '0 20px' }}>
      <Steps current={currentStep} items={[
        { title: 'Thông tin khám', icon: <SolutionOutlined /> },
        { title: 'Chọn ngày giờ', icon: <CalendarOutlined /> },
        { title: 'Hoàn tất', icon: <UserOutlined /> },
      ]} style={{ marginBottom: 40 }} />
      
      {renderStepContent()}
    </div>
  );
};

export default BookingPage;