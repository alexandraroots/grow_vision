import numpy as np
import cv2
from tqdm import tqdm
import os

def camera_calibration():
    criteria = (cv2.TERM_CRITERIA_EPS + cv2.TERM_CRITERIA_MAX_ITER, 30, 0.001)

    objp = np.zeros((6*9, 3), np.float32)
    objp[:,:2] = np.mgrid[0:9, 0:6].T.reshape(-1, 2)

    objpoints, imgpoints = [], []

    calibration_img = ['./data/image_2021_03_01T20_26_33_033Z.png', './data/image_2021_03_01T20_26_51_613Z.png',
                        './data/image_2021_03_01T20_24_39_871Z.png', './data/image_2021_03_01T20_25_02_541Z.png']

    for img_path in calibration_img:

        img = cv2.imread(img_path)
        gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        ret, corners = cv2.findChessboardCorners(gray, (9, 6), None)

        if ret == True:
            objpoints.append(objp)

            corners2 = cv2.cornerSubPix(gray, corners, (11, 11), (-1, -1), criteria)
            imgpoints.append(corners2)

            #img = cv2.drawChessboardCorners(img, (9, 6), corners2, ret)
        #cv2.imwrite('./data/' + str(i) + '.png', img)

    return objpoints, imgpoints, gray

if __name__ == "__main__":

    objpoints, imgpoints, gray = camera_calibration()
    ret, mtx, dist, rvecs, tvecs = cv2.calibrateCamera(objpoints, imgpoints, gray.shape[::-1], None, None)

    source_dir = './data/timelapse1/'
    for i, file_name in enumerate(tqdm(os.listdir(source_dir))):
        if file_name[0] == '.':
            continue
        img = cv2.imread(source_dir + file_name)
        h,  w = img.shape[:2]

        newcameramtx, roi = cv2.getOptimalNewCameraMatrix(mtx, dist, (w, h), 1, (w, h))

        # undistort
        dst = cv2.undistort(img, mtx, dist, None, newcameramtx)
        x, y, w, h = roi
        dst = dst[y:y+h, x:x+w]

        #обрезка лишнего
        dst = dst[70:dst.shape[0] - 80, 620:1570]
        cv2.imwrite('./data/timelapse1_crop/' + file_name, dst)