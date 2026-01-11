import React, { useEffect, useState, useRef } from 'react';
import { Carousel, Button, Typography, Row, Col, Card, Spin, Tag, Statistic } from 'antd';
import { 
  CalendarOutlined, 
  MedicineBoxOutlined, 
  PhoneOutlined, 
  SafetyCertificateOutlined, 
  TeamOutlined, 
  ClockCircleOutlined,
  LeftOutlined,
  RightOutlined
} from '@ant-design/icons';
import { Link } from 'react-router-dom';
import axios from '../services/axios';

const { Title, Text, Paragraph } = Typography;
const { Meta } = Card;

const HomePage = () => {
  const [doctors, setDoctors] = useState([]);
  const [loading, setLoading] = useState(true);
  
  // Ref để điều khiển Carousel
  const carouselRef = useRef(null);

  // 1. Gọi API lấy danh sách bác sĩ
  useEffect(() => {
    const fetchDoctors = async () => {
      try {
        const data = await axios.get('/Doctor'); // Gọi API DoctorController
        setDoctors(data);
      } catch (error) {
        console.error("Lỗi lấy danh sách bác sĩ:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchDoctors();
  }, []);

  // Cấu hình Carousel
  const carouselSettings = {
    dots: false,
    infinite: true,
    speed: 500,
    slidesToShow: 4,
    slidesToScroll: 1,
    draggable: true,
    responsive: [
      { breakpoint: 1024, settings: { slidesToShow: 3 } },
      { breakpoint: 768, settings: { slidesToShow: 2 } },
      { breakpoint: 480, settings: { slidesToShow: 1 } }
    ]
  };

  // Style cho Banner
  const heroStyle = {
    height: '500px',
    color: '#fff',
    textAlign: 'center',
    background: 'url(https://img.freepik.com/free-photo/blur-hospital_1203-7972.jpg?w=1380) center/cover no-repeat',
    position: 'relative',
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
  };

  const overlayStyle = {
    position: 'absolute',
    top: 0,
    left: 0,
    width: '100%',
    height: '100%',
    backgroundColor: 'rgba(3, 169, 244, 0.6)', // LightBlue overlay
  };

  return (
    <div className="homepage">
      {/* === SECTION 1: HERO BANNER === */}
      <div style={heroStyle}>
        <div style={overlayStyle}></div>
        <div style={{ position: 'relative', zIndex: 1, maxWidth: '800px', padding: '0 20px' }}>
          <Title level={1} style={{ color: '#fff', marginBottom: '10px', fontSize: '48px', fontWeight: 'bold' }}>
            CHĂM SÓC SỨC KHỎE TOÀN DIỆN
          </Title>
          <Paragraph style={{ color: '#fff', fontSize: '18px', marginBottom: '30px' }}>
            Đặt lịch khám nhanh chóng - Đội ngũ bác sĩ chuyên môn cao - Dịch vụ tận tâm
          </Paragraph>
          
          <Link to="/dat-lich">
            <Button 
              type="primary" 
              size="large" 
              icon={<CalendarOutlined />}
              style={{ 
                backgroundColor: '#cddc39', // Lime Color
                borderColor: '#cddc39', 
                color: '#000', 
                fontWeight: 'bold',
                height: '50px',
                padding: '0 40px',
                fontSize: '16px'
              }}
            >
              ĐẶT LỊCH KHÁM NGAY
            </Button>
          </Link>
        </div>
      </div>

      {/* === SECTION 2: DỊCH VỤ NỔI BẬT === */}
      <div style={{ padding: '60px 50px', backgroundColor: '#fff' }}>
        <div style={{ textAlign: 'center', marginBottom: '50px' }}>
          <Title level={2} style={{ color: '#03a9f4' }}>Dịch Vụ Của Chúng Tôi</Title>
          <Text type="secondary">Giải pháp y tế tiên tiến nhất dành cho bạn và gia đình</Text>
        </div>

        <Row gutter={[32, 32]} justify="center">
          <Col xs={24} md={8}>
            <Card hoverable style={{ textAlign: 'center', height: '100%', borderTop: '4px solid #03a9f4' }}>
              <MedicineBoxOutlined style={{ fontSize: '48px', color: '#03a9f4', marginBottom: '20px' }} />
              <Title level={4}>Khám & Điều Trị</Title>
              <Paragraph>Đa dạng chuyên khoa: Nội, Ngoại, Nhi, Sản... với phác đồ điều trị chuẩn quốc tế.</Paragraph>
            </Card>
          </Col>
          <Col xs={24} md={8}>
            <Card hoverable style={{ textAlign: 'center', height: '100%', borderTop: '4px solid #cddc39' }}>
              <SafetyCertificateOutlined style={{ fontSize: '48px', color: '#afb42b', marginBottom: '20px' }} />
              <Title level={4}>Xét Nghiệm & Chẩn Đoán</Title>
              <Paragraph>Hệ thống máy móc hiện đại, trả kết quả nhanh chóng và chính xác tuyệt đối.</Paragraph>
            </Card>
          </Col>
          <Col xs={24} md={8}>
            <Card hoverable style={{ textAlign: 'center', height: '100%', borderTop: '4px solid #03a9f4' }}>
              <ClockCircleOutlined style={{ fontSize: '48px', color: '#03a9f4', marginBottom: '20px' }} />
              <Title level={4}>Hỗ Trợ 24/7</Title>
              <Paragraph>Đội ngũ tư vấn và cấp cứu luôn sẵn sàng hỗ trợ bạn mọi lúc, mọi nơi.</Paragraph>
            </Card>
          </Col>
        </Row>
      </div>

      {/* === SECTION 3: ĐỘI NGŨ BÁC SĨ (CAROUSEL SLIDER) === */}
      <div style={{ padding: '60px 50px', backgroundColor: '#f0f2f5' }}>
        
        {/* Header Section: Title + Navigation Buttons */}
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '30px' }}>
          <div>
            <Title level={2} style={{ color: '#03a9f4', margin: 0 }}>Đội Ngũ Chuyên Gia</Title>
            <Text type="secondary">Gặp gỡ những bác sĩ hàng đầu của chúng tôi</Text>
          </div>
          
          <div style={{ display: 'flex', gap: '10px' }}>
            <Button 
              shape="circle" 
              icon={<LeftOutlined />} 
              onClick={() => carouselRef.current.prev()} 
              style={{ border: '1px solid #03a9f4', color: '#03a9f4' }}
            />
            <Button 
              shape="circle" 
              icon={<RightOutlined />} 
              onClick={() => carouselRef.current.next()} 
              type="primary"
              style={{ backgroundColor: '#03a9f4' }}
            />
          </div>
        </div>

        {loading ? (
          <div style={{ textAlign: 'center', padding: '50px' }}><Spin size="large" /></div>
        ) : (
          /* Carousel Component */
          <div style={{ margin: '0 -10px' }}>
            <Carousel ref={carouselRef} {...carouselSettings}>
              {doctors.map((doc) => (
                <div key={doc.maNhanVien} style={{ padding: '0 10px' }}>
                  <Card
                    hoverable
                    style={{ margin: '10px' }}
                    cover={
                      <img
                        alt={doc.hoTen}
                        src={doc.hinhAnh || "https://via.placeholder.com/300x300?text=Doctor"}
                        style={{ height: '250px', objectFit: 'cover', objectPosition: 'top', width: '100%' }}
                      />
                    }
                    actions={[
                      <Link to="/dat-lich" state={{ doctor: doc }}>
                        <Button type="link" icon={<CalendarOutlined />}>Đặt lịch</Button>
                      </Link>,
                    ]}
                  >
                    <Meta
                      title={<span style={{ color: '#03a9f4' }}>{doc.hoTen}</span>}
                      description={
                        <div>
                          <Tag color="blue">{doc.chuyenKhoa}</Tag>
                          <div style={{ marginTop: '10px', fontSize: '12px' }}>
                             <PhoneOutlined /> {doc.soDienThoai}
                          </div>
                        </div>
                      }
                    />
                  </Card>
                </div>
              ))}
            </Carousel>
          </div>
        )}
      </div>

      {/* === SECTION 4: THỐNG KÊ === */}
      <div style={{ backgroundColor: '#fff', padding: '50px' }}>
        <Row gutter={16} style={{ textAlign: 'center' }}>
          <Col span={8}>
            <Statistic title="Lượt khám bệnh" value={112893} prefix={<TeamOutlined />} />
          </Col>
          <Col span={8}>
            <Statistic title="Bác sĩ chuyên khoa" value={doctors.length} suffix="+" />
          </Col>
          <Col span={8}>
            <Statistic title="Hài lòng" value={99.9} suffix="%" />
          </Col>
        </Row>
      </div>
    </div>
  );
};

export default HomePage;